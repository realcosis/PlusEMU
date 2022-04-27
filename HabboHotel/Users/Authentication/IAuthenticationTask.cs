using System.Threading.Tasks;

namespace Plus.HabboHotel.Users.Authentication
{
    public interface IAuthenticationTask
    {
        Task<bool> CanLogin(int userId) => Task.FromResult(true);
        Task UserLoggedIn(Habbo habbo) => Task.CompletedTask;
        Task UserLoggedOut(Habbo habbo) => Task.CompletedTask;
    }
}