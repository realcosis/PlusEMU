namespace Plus.HabboHotel.Badges;

public class BadgeDefinition
{
    public BadgeDefinition(string code, string requiredRight)
    {
        Code = code;
        RequiredRight = requiredRight;
    }

    public string Code { get; }
    public string RequiredRight { get; }
}