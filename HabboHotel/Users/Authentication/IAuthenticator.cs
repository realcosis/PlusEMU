using System.Threading.Tasks;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Users.Authentication
{
    public interface IAuthenticator
    {
        Task<AuthenticationError?> AuthenticateUsingSSO(GameClient session, string sso);
    }
}