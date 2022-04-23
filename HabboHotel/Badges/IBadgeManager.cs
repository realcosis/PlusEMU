namespace Plus.HabboHotel.Badges;

public interface IBadgeManager
{
    void Init();
    bool TryGetBadge(string code, out BadgeDefinition badge);
}