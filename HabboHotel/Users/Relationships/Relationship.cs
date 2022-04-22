namespace Plus.HabboHotel.Users.Relationships
{
    public class Relationship
    {
        public int Id;
        public int Type;
        public int UserId;

        public Relationship(int id, int user, int type)
        {
            this.Id = id;
            UserId = user;
            this.Type = type;
        }
    }
}