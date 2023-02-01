using Plus.Core.Language;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Moderation;

internal class ModerationKickEvent : IPacketEvent
{
    private readonly IGameClientManager _clientManager;
    private readonly ILanguageManager _languageManager;

    public ModerationKickEvent(IGameClientManager clientManager, ILanguageManager languageManager)
    {
        _clientManager = clientManager;
        _languageManager = languageManager;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        if (!session.GetHabbo().GetPermissions().HasRight("mod_kick"))
            return Task.CompletedTask;
        var userId = packet.ReadInt();
        packet.ReadString(); //message
        var client = _clientManager.GetClientByUserId(userId);
        if (client == null || client.GetHabbo() == null || client.GetHabbo().CurrentRoom == null || client.GetHabbo().Id == session.GetHabbo().Id)
            return Task.CompletedTask;
        if (client.GetHabbo().Rank >= session.GetHabbo().Rank)
        {
            session.SendNotification(_languageManager.TryGetValue("moderation.kick.disallowed"));
            return Task.CompletedTask;
        }
        session.GetHabbo().CurrentRoom?.GetRoomUserManager().RemoveUserFromRoom(client, true);
        return Task.CompletedTask;
    }
}