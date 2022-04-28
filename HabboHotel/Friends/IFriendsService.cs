using System.Collections.Generic;
using System.Threading.Tasks;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.HabboHotel.Friends
{
    public interface IFriendsService
    {
        Task<List<MessengerBuddy>> GetBuddiesForUser(int userId);
        Task<List<MessengerRequest>> GetRequestsForUser(int userId);
    }
}