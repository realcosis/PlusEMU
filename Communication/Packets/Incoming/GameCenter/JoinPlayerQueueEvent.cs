using System;
using System.Text;
using Plus.Communication.Packets.Outgoing.GameCenter;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Games;

namespace Plus.Communication.Packets.Incoming.GameCenter;

internal class JoinPlayerQueueEvent : IPacketEvent
{
    private readonly IGameDataManager _gameDataManager;

    public JoinPlayerQueueEvent(IGameDataManager gameDataManager)
    {
        _gameDataManager = gameDataManager;
    }
    public void Parse(GameClient session, ClientPacket packet)
    {
        var gameId = packet.PopInt();
        GameData gameData = null;
        if (_gameDataManager.TryGetGame(gameId, out gameData))
        {
            var ssoTicket = "HABBOON-Fastfood-" + GenerateSso(32) + "-" + session.GetHabbo().Id;
            session.SendPacket(new JoinQueueComposer(gameData.Id));
            session.SendPacket(new LoadGameComposer(gameData, ssoTicket));
        }
    }

    private string GenerateSso(int length)
    {
        var characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        var result = new StringBuilder(length);
        for (var i = 0; i < length; i++) result.Append(characters[Random.Shared.Next(characters.Length)]);
        return result.ToString();
    }
}