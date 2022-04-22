namespace Plus.HabboHotel.Users.Badges
{
    public class Badge
    {
        public string Code;
        public int Slot;

        public Badge(string code, int slot)
        {
            this.Code = code;
            this.Slot = slot;
        }
    }
}