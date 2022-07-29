using Plus.Utilities.DependencyInjection;

namespace Plus.Core
{
    [Singleton]
    public interface IStartable
    {
        Task Start();
    }
}
