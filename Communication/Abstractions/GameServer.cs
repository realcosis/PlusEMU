using Microsoft.Extensions.Options;
using NetCoreServer;
using Plus.Communication.Packets;
using Plus.HabboHotel.GameClients;
using Plus.Utilities.DependencyInjection;
using System.Collections.Concurrent;
using Plus.Communication.Flash;

namespace Plus.Communication.Abstractions
{
    public interface IGameServerOptions
    {
        string Name { get; }
        string Hostname { get; }
        int Port { get; }
    }

    public abstract class GameServer<TGameServerOptions, TGameClient> : TcpServer, IGameServer
        where TGameServerOptions : class, IGameServerOptions
        where TGameClient : GameClient
    {
        private readonly IGameClientFactory<TGameClient> _clientFactory;
        private readonly IPacketManager _packetManager;
        private readonly ConcurrentDictionary<Guid, TGameClient> _connectedClients = new();

        protected GameServer(IOptions<TGameServerOptions> options,
            IGameClientFactory<TGameClient> clientFactory,
            IPacketManager packetManager) : base(options.Value.Hostname,
            options.Value.Port)
        {
            _clientFactory = clientFactory;
            _packetManager = packetManager;
        }

        protected override TcpSession CreateSession() => _clientFactory.Create(this);

        protected override void OnConnected(TcpSession session)
        {
            if (session is not TGameClient gameClient)
            {
                session.Disconnect();
                //_logger.LogWarning("Expected {TGameClient} to be connected. Got {type}", typeof(TGameClient), session.GetType());
                return;
            }

            if (!_connectedClients.TryAdd(gameClient.Id, gameClient))
            {
                //_logger.LogWarning("Failed to cache client. {id} {ip}", gameClient.Id, gameClient.Socket.RemoteEndPoint?.ToString());
                gameClient.Disconnect();
            }
        }

        // TODO @80O: Multi revision support.
        // TODO @80O: Allow packet content to be modified before executing.
        // TODO @80O: Add hooks before & after packet execution.
        public Task PacketReceived(GameClient client, uint messageId, IIncomingPacket packet) => _packetManager.TryExecutePacket(client, messageId, packet);
    }

    [Transient]
    public interface IGameClientFactory
    {
    }
    public interface IGameClientFactory<TGameClient> : IGameClientFactory where TGameClient : GameClient
    {
        TGameClient Create(TcpServer server);
    }
}
