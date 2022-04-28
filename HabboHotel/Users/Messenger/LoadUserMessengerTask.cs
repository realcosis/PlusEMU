using Plus.HabboHotel.Friends;
using Plus.HabboHotel.Users.UserData;
using System.Linq;
using System.Threading.Tasks;

namespace Plus.HabboHotel.Users.Messenger
{
    public class LoadUserMessengerTask : IUserDataLoadingTask
    {
        private readonly IFriendsService _friendsService;

        public LoadUserMessengerTask(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public async Task Load(Habbo habbo)
        {
            var messenger = new HabboMessenger(habbo.Id);
            messenger.Init((await _friendsService.GetBuddiesForUser(habbo.Id)).ToDictionary(buddy => buddy.Id), (await _friendsService.GetRequestsForUser(habbo.Id)).ToDictionary(request => request.FromId));
            habbo.SetMessenger(messenger);
        }
    }
}
