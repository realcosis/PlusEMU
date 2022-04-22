using Plus.Utilities;

namespace Plus.HabboHotel.Rewards;

public class Reward
{
    public Reward(double start, double end, string type, string rewardData, string message)
    {
        RewardStart = start;
        RewardEnd = end;
        Type = RewardTypeUtility.GetType(type);
        RewardData = rewardData;
        Message = message;
    }

    public double RewardStart { get; set; }
    public double RewardEnd { get; set; }
    public RewardType Type { get; set; }
    public string RewardData { get; set; }
    public string Message { get; set; }

    public bool Active
    {
        get
        {
            var now = UnixTimestamp.GetNow();
            return now >= RewardStart && now <= RewardEnd;
        }
    }
}