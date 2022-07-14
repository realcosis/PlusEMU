using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using NetCoreServer;
using Plus.Communication.Abstractions;
using Plus.Communication.Packets;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Flash
{
    public static class PlusMemoryStream
    {
        public static readonly RecyclableMemoryStreamManager Manager = new();

        public static RecyclableMemoryStream GetStream() => (Manager.GetStream() as RecyclableMemoryStream)!;
        public static RecyclableMemoryStream GetStream(ReadOnlySpan<byte> buffer) => (Manager.GetStream(buffer) as RecyclableMemoryStream)!;
    }

    public interface IGameServer
    {
        bool Start();
        bool Stop();

        Task PacketReceived(GameClient client, uint messageId, IIncomingPacket packet);
    }


    public interface IFlashServer : IGameServer
    {

    }

    public class FlashRevision
    {
        public string Revision { get; set; } = string.Empty;
        public Dictionary<int, int> HeaderMapping { get; set; } = new();
    }

    public class FlashServer : GameServer<FlashServerConfiguration, FlashGameClient>, IFlashServer
    {
        public FlashServer(IOptions<FlashServerConfiguration> options, FlashClientFactory flashClientFactory, IPacketManager packetManager) : base(options, flashClientFactory, packetManager)
        {
        }
    }

    public class FlashPacketFactory : IPacketFactory
    {
        public IIncomingPacket CreateIncomingPacket(Memory<byte> buffer) => new FlashIncomingPacket { Buffer = buffer };

        public IOutgoingPacket CreateOutgoingPacket(RecyclableMemoryStream stream) => new FlashOutgoingPacket(stream);
    }

    public class FlashClientFactory : IGameClientFactory<FlashGameClient>
    {
        private readonly FlashPacketFactory _packetFactory;

        public FlashClientFactory(FlashPacketFactory packetFactory)
        {
            _packetFactory = packetFactory;
        }
        public FlashGameClient Create(TcpServer server) => new(server as FlashServer, _packetFactory);
    }

    public class FlashOutgoingPacket : IOutgoingPacket
    {
        private readonly RecyclableMemoryStream _stream;
        public int MessageId { get; set; }

        public FlashOutgoingPacket(RecyclableMemoryStream stream)
        {
            _stream = stream;
            _stream.Advance(6);
        }

        public ReadOnlyMemory<byte> Buffer => _stream.GetMemory();
        public void WriteByte(byte value) => _stream.WriteByte(value);

        public void WriteShort(short value)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(sizeof(short));
            var span = buffer.AsSpan();
            if (BitConverter.IsLittleEndian)
                value = BinaryPrimitives.ReverseEndianness(value);
            MemoryMarshal.Write(span, ref value);
            _stream.Write(span.Slice(0, sizeof(short)));
            ArrayPool<byte>.Shared.Return(buffer);
        }

        public void WriteInt(int value)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(sizeof(int));
            var span = buffer.AsSpan();
            if (BitConverter.IsLittleEndian)
                value = BinaryPrimitives.ReverseEndianness(value);
            MemoryMarshal.Write(span, ref value);
            _stream.Write(span.Slice(0, sizeof(int)));
            ArrayPool<byte>.Shared.Return(buffer);
        }

        public void WriteInteger(int value) => WriteInt(value);

        public void WriteBool(bool value) => WriteByte(value ? (byte)1 : (byte)0);
        public void WriteBoolean(bool value) => WriteBool(value);

        public void WriteString(string value)
        {
            var buffer = !string.IsNullOrEmpty(value) ? Encoding.UTF8.GetBytes(value) : Array.Empty<byte>();

            if (buffer.Length <= ushort.MaxValue)
            {
                WriteShort((short)buffer.Length);
                if (buffer.Length > 0)
                    _stream.Write(buffer, 0, buffer.Length);
            }
        }

        public void WriteDouble(double value)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(sizeof(double));
            var span = buffer.AsSpan();
            if (BitConverter.IsLittleEndian)
            {
                var dSpan = new Span<byte>(BitConverter.GetBytes(value));
                dSpan.Reverse();
                value = BitConverter.ToDouble(dSpan);
            }
            MemoryMarshal.Write(span, ref value);
            _stream.Write(span.Slice(0, sizeof(double)));
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public class FlashIncomingPacket : IIncomingPacket
    {
        public Memory<byte> Buffer { get; set; }
        public int MessageId { get; set; }

        public byte ReadByte()
        {
            var span = Buffer.Span;
            var result = MemoryMarshal.Read<byte>(span);
            Buffer = Buffer.Slice(sizeof(byte));
            return result;
        }

        public short ReadShort()
        {
            var span = Buffer.Span.Slice(0, 2);
            span.Reverse();
            var result = MemoryMarshal.Read<short>(span);
            Buffer = Buffer.Slice(sizeof(short));
            return result;
        }
        public ushort ReadUShort()
        {
            var span = Buffer.Span.Slice(0, 2);
            span.Reverse();
            var result = MemoryMarshal.Read<ushort>(span);
            Buffer = Buffer.Slice(sizeof(ushort));
            return result;
        }

        public int ReadInt()
        {
            var span = Buffer.Span.Slice(0, 4);
            span.Reverse();
            var result = MemoryMarshal.Read<int>(span);
            Buffer = Buffer.Slice(sizeof(int));
            return result;
        }

        public bool ReadBool() => ReadInt() == 1;

        public string ReadString()
        {
            var length = ReadUShort();
            var value = System.Text.Encoding.UTF8.GetString(Buffer.Span.Slice(0, length));
            Buffer = Buffer.Slice(length);
            return value;
        }

        public bool HasDataRemaining() => !Buffer.IsEmpty;
        public byte[] ReadFixedValue()
        {
            var length = ReadUShort();
            var span = Buffer.Slice(0, length);
            Buffer = Buffer.Slice(length);
            return span.ToArray();
        }

        public void ReadBytes(Span<byte> destination) => throw new NotImplementedException();
    }

    public class FlashGameClient : GameClient
    {
        private bool _hasReceivedPolicy;
        private static byte[] XmlPolicy = Encoding.UTF8.GetBytes("<?xml version=\"1.0\"?>\r\n" +
                                                                 "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                                                                 "<cross-domain-policy>\r\n" +
                                                                 "<allow-access-from domain=\"*\" to-ports=\"1-31111\" />\r\n" +
                                                                 "</cross-domain-policy>\x0");
        public FlashGameClient(FlashServer server, IPacketFactory packetFactory) : base(server, packetFactory)
        {
        }

        public static int DecodeInt32(ReadOnlyMemory<byte> v) => (v.Span[0] << 24) | (v.Span[1] << 16) | (v.Span[2] << 8) | v.Span[3];
        public static int DecodeInt16(ReadOnlyMemory<byte> v) => (v.Span[0] << 8) | v.Span[1];

        public static void EncodeInt32(Memory<byte> memory, int value, int offset)
        {
            var span = memory.Span;
            span[offset + 0] = (byte)((value >> 24) & 0xFF);
            span[offset + 1] = (byte)((value >> 16) & 0xFF);
            span[offset + 2] = (byte)((value >> 8) & 0xFF);
            span[offset + 3] = (byte)((value >> 0) & 0xFF);
        }
        public static void EncodeInt16(Memory<byte> memory, short value, int offset)
        {
            var span = memory.Span;
            span[offset + 0] = (byte)((value >> 8) & 0xFF);
            span[offset + 1] = (byte)((value >> 0) & 0xFF);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (!_hasReceivedPolicy && buffer[offset] == (byte)'<')
            {
                _hasReceivedPolicy = true;
                Send(XmlPolicy);
                Disconnect();
                return;
            }

            base.OnReceived(buffer, offset, size);
        }

        public override void CreateHeader(Memory<byte> memory, int messageId)
        {
            EncodeInt32(memory, memory.Length - 4, 0);
            EncodeInt16(memory, (short)messageId, 4);
        }

        public override (bool Complete, uint MessageId, int HeaderLength, int Length) GetMessageIdAndPacketLength(ReadOnlyMemory<byte> buffer)
        {
            if (buffer.Length < 6) return default;
            var length = DecodeInt32(buffer) - 2;
            if (length + 6 > buffer.Length) return default;
            buffer = buffer.Slice(4);
            var messageId = (uint)DecodeInt16(buffer);
            return (true, messageId, 6, length);
        }
    }
}
