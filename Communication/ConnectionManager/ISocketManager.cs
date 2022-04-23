namespace Plus.Communication.ConnectionManager;

public interface ISocketManager
{
    /// <summary>
    ///     Occurs when a new connection was established
    /// </summary>
    event SocketManager.ConnectionEvent OnConnectionEvent;

    /// <summary>
    ///     Initializes the connection instance
    /// </summary>
    /// <param name="portId">The ID of the port this item should listen on</param>
    /// <param name="maxConnections">The maximum amount of connections</param>
    /// <param name="connectionsPerIp">The maximum allowed connections per IP Address</param>
    /// <param name="parser">The data parser for the connection</param>
    /// <param name="disableNaglesAlgorithm">Disable nagles algorithm</param>
    void Init(int portId, int maxConnections, int connectionsPerIp, IDataParser parser, bool disableNaglesAlgorithm);

    /// <summary>
    ///     Initializes the incoming data requests
    /// </summary>
    void InitializeConnectionRequests();

    /// <summary>
    ///     Destroys the current connection manager and disconnects all users
    /// </summary>
    void Destroy();

    /// <summary>
    ///     Reports a gameconnection as disconnected
    /// </summary>
    /// <param name="gameConnection">The connection which is logging out</param>
    void ReportDisconnect(ConnectionInformation gameConnection);
}