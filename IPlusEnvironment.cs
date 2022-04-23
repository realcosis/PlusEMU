using System.Threading.Tasks;

namespace Plus;

public interface IPlusEnvironment
{
    Task<bool> Start();
}