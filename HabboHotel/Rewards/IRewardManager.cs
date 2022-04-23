using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rewards;

public interface IRewardManager
{
    void Init();
    bool HasReward(int id, int rewardId);
    void LogReward(int id, int rewardId);
    void CheckRewards(GameClient session);
}