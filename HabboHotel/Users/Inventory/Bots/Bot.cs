namespace Plus.HabboHotel.Users.Inventory.Bots
{
    public class Bot
    {
        private int _id;
        private int _ownerId;
        private string _name;
        private string _motto;
        private string _figure;
        private string _gender;

        public Bot(int id, int ownerId, string name, string motto, string figure, string gender)
        {
            this.Id = id;
            this.OwnerId = ownerId;
            this.Name = name;
            this.Motto = motto;
            this.Figure = figure;
            this.Gender = gender;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int OwnerId
        {
            get { return _ownerId; }
            set { _ownerId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Motto
        {
            get { return _motto; }
            set { _motto = value; }
        }

        public string Figure
        {
            get { return _figure; }
            set { _figure = value; }
        }

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
    }
}
