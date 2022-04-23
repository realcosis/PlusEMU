namespace Plus.Communication.ConnectionManager;

public interface IConnectionHandling
{
    void Init(int port, int maxConnections, int connectionsPerIp, bool enabeNagles);
    void Destroy();
}