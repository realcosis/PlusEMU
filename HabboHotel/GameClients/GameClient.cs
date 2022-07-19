using System.Net.Sockets;
using Microsoft.IO;
using NetCoreServer;
using NLog;
using Plus.Communication.Encryption.Crypto.Prng;
using Plus.Communication.Flash;
using Plus.Communication.Packets;
using Plus.HabboHotel.Users;

namespace Plus.HabboHotel.GameClients;

public abstract class GameClient : TcpSession
{
    private readonly IGameServer _server;
    private readonly IPacketFactory _packetFactory;
    private Habbo? _habbo;
    public event EventHandler<EventArgs>? ConnectionConnected;
    public event EventHandler<EventArgs>? ConnectionDisconnected;

    public RecyclableMemoryStream? _incompleteStream;
    public Arc4? Rc4Client { get; set; }

    public bool IsAuthenticated { get; set; } = false;
    public DateTime TimeConnected { get; set; }

    [Obsolete("Will be removed")]
    public string MachineId { get; set; } = string.Empty;

    [Obsolete("Will be removed")]
    public int PingCount { get; set; }

    protected GameClient(IGameServer server, IPacketFactory packetFactory) : base(server as TcpServer)
    {
        _server = server;
        _packetFactory = packetFactory;
    }

    protected override void OnConnected()
    {
        Socket.DontFragment = true;
        TimeConnected = DateTime.UtcNow;
        ConnectionConnected?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnDisconnected() => ConnectionDisconnected?.Invoke(this, EventArgs.Empty);

    public abstract (bool Complete, uint MessageId, int HeaderLength, int Length) GetMessageIdAndPacketLength(ReadOnlyMemory<byte> buffer);

    protected override async void OnReceived(byte[] buffer, long offset, long size)
    {
        if (size > int.MaxValue) throw new InvalidOperationException("");
        await using var stream = PlusMemoryStream.GetStream(buffer.AsSpan().Slice((int) offset, (int) size));
        var memory = stream.GetMemory().Slice(0, (int)stream.Length);

        if (_incompleteStream != null)
        {
            _incompleteStream.Write(memory.Span);
            memory = _incompleteStream.GetMemory().Slice(0, (int)_incompleteStream.Length);
        }

        while (memory.Length > 0)
        {
            var (complete, messageId, headerLength, length) = GetMessageIdAndPacketLength(memory);
            if (!complete)
            {
                _incompleteStream ??= PlusMemoryStream.GetStream(memory.Span);
                break;
            }

            try
            {
                await _server.PacketReceived(this, messageId, _packetFactory.CreateIncomingPacket(memory.Slice(headerLength, length)));
            }
            catch (Exception e)
            {
                // TODO @80O: Add logging when ILogger interface has been implemented
            }
            memory = memory.Slice(headerLength + length);
            _incompleteStream?.Advance(headerLength + length);
        }

        if (memory.Length == 0)
        {
            _incompleteStream?.Dispose();
            _incompleteStream = null;
        }
    }

    public Habbo GetHabbo() => _habbo!;

    public void SetHabbo(Habbo habbo)
    {
        if (_habbo != null) throw new InvalidOperationException();
        _habbo = habbo;
    }

    private static readonly ILogger Log = LogManager.GetLogger("Plus.Communication.Packets");

    public void Send(IServerPacket composer)
    {
        var stream = PlusMemoryStream.GetStream();
        stream.Position = 0;
        var packet = _packetFactory.CreateOutgoingPacket(stream);
        composer.Compose(packet);
        var args = new SocketAsyncEventArgs();
        var memory = stream.GetBuffer().AsMemory().Slice(0, (int)stream.Length);
        CreateHeader(memory, composer.MessageId);
        args.SetBuffer(memory);
        Socket.SendAsync(args);
        Log.Debug("Send Packet: [" + composer.MessageId + "] " + composer.GetType().Name);
        stream.Dispose();
    }

    public abstract void CreateHeader(Memory<byte> memory, int messageId);
}
