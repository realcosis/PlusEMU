namespace Plus.HabboHotel.Users.Ignores
{
    public interface IIgnoredUsersService
    {
        Task<List<string>> GetIgnoredUsersByName(IEnumerable<int> userIds);
    }
}