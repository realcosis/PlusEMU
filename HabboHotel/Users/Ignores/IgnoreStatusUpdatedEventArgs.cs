namespace Plus.HabboHotel.Users.Ignores
{
    public class IgnoreStatusUpdatedEventArgs : EventArgs
    {
        public IgnoreStatusUpdatedEventArgs(uint userId)
        {
            UserId = userId;
        }
        public uint UserId { get; }
    }
}