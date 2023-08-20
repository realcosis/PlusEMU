using Dapper;
using Plus.Communication.Packets.Outgoing.Users;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Quests;

namespace Plus.Communication.Packets.Incoming.Inventory.Badges;

internal class SetActivatedBadgesEvent : IPacketEvent
{
    private readonly IQuestManager _questManager;
    private readonly IDatabase _database;

    public SetActivatedBadgesEvent(IQuestManager questManager, IDatabase database)
    {
        _questManager = questManager;
        _database = database;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var habbo = session.GetHabbo();
        habbo.Inventory.Badges.ClearWearingBadges();

        var updates = new List<dynamic>();

        for (var i = 0; i < 5; i++)
        {
            var slot = packet.ReadInt();
            var badge = packet.ReadString();

            if (string.IsNullOrEmpty(badge) ||
                !habbo.Inventory.Badges.HasBadge(badge) ||
                slot < 1 || slot > 5)
            {
                continue;
            }

            habbo.Inventory.Badges.GetBadge(badge).Slot = slot;
            updates.Add(new { slot, badge, userId = habbo.Id });
        }

        using (var connection = _database.Connection())
        {
            // Reset all badge slots to 0 for the user
            await connection.ExecuteAsync("UPDATE `user_badges` SET `badge_slot` = '0' WHERE `user_id` = @userId", new { userId = habbo.Id });

            // Update the badge slots
            await connection.ExecuteAsync("UPDATE `user_badges` SET `badge_slot` = @slot WHERE `badge_id` = @badge AND `user_id` = @userId LIMIT 1", updates);
        }

        _questManager.ProgressUserQuest(session, QuestType.ProfileBadge);

        var equippedBadges = habbo.Inventory.Badges.EquippedBadges;

        if (habbo.InRoom)
        {
            habbo.CurrentRoom?.SendPacket(new HabboUserBadgesComposer(habbo.Id, equippedBadges));
        }
        else
        {
            session.Send(new HabboUserBadgesComposer(habbo.Id, equippedBadges));
        }
    }
}
