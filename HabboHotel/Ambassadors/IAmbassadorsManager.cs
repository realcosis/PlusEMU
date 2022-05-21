using System.Threading.Tasks;

namespace Plus.HabboHotel.Ambassadors
{
    public interface IAmbassadorsManager
    {
        Task AddLogs(int userid, string target, string type);
    }
}
