using System;
using System.Text;
using Plus.Communication.Packets.Outgoing.GameCenter;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Games;

namespace Plus.Communication.Packets.Incoming.GameCenter;

internal class JoinPlayerQueueEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session == null || session.GetHabbo() == null)
            return;
        var gameId = packet.PopInt();
        GameData gameData = null;
        if (PlusEnvironment.GetGame().GetGameDataManager().TryGetGame(gameId, out gameData))
        {
            var ssoTicket = "HABBOON-Fastfood-" + GenerateSso(32) + "-" + session.GetHabbo().Id;
            session.SendPacket(new JoinQueueComposer(gameData.Id));
            session.SendPacket(new LoadGameComposer(gameData, ssoTicket));
        }
    }

    private string GenerateSso(int length)
    {
        var random = new Random();
        var characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        var result = new StringBuilder(length);
        for (var i = 0; i < length; i++) result.Append(characters[random.Next(characters.Length)]);
        return result.ToString();
    }
}