using Plus.Utilities.DependencyInjection;

namespace Plus.Plugins
{
    [Transient]
    public interface IPlugin
    {
        void Start();
    }
}