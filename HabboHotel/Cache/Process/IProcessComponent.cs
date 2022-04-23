namespace Plus.HabboHotel.Cache.Process;

public interface IProcessComponent
{
    /// <summary>
    /// Initializes the ProcessComponent.
    /// </summary>
    void Init();

    /// <summary>
    /// Called for each time the timer ticks.
    /// </summary>
    /// <param name="state"></param>
    void Run(object state);

    /// <summary>
    /// Stops the timer and disposes everything.
    /// </summary>
    void Dispose();
}