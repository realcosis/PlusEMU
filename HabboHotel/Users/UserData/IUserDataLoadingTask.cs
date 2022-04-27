using System.Threading.Tasks;

namespace Plus.HabboHotel.Users.UserData
{
    public interface IUserDataLoadingTask
    {
        /// <summary>
        /// Decorate the Habbo object.
        /// UserId as well as username is set.
        /// </summary>
        /// <param name="habbo"></param>
        /// <returns></returns>
        Task Load(Habbo habbo);
    }
}