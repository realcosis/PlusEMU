namespace Plus.HabboHotel.Users;

public interface IHabboStatsService
{
    Task<HabboStats> LoadHabboStats(int userId);
}
