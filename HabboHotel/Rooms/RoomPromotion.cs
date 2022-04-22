using System;

namespace Plus.HabboHotel.Rooms
{
    public class RoomPromotion
    {
        private string _name;
        private string _description;
        private double _timestampExpires;
        private double _timestampStarted;
        private int _categoryId;

        public RoomPromotion(string name, string description, int categoryId)
        {
            _name = name;
            _description = description;
            _timestampStarted = PlusEnvironment.GetUnixTimestamp();
            _timestampExpires = (PlusEnvironment.GetUnixTimestamp()) + (Convert.ToInt32(PlusEnvironment.GetSettingsManager().TryGetValue("room.promotion.lifespan")) * 60);
            _categoryId = categoryId;
        }

        public RoomPromotion(string name, string description, double started, double expires, int categoryId)
        {
            _name = name;
            _description = description;
            _timestampStarted = started;
            _timestampExpires = expires;
            _categoryId = categoryId;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public double TimestampStarted => _timestampStarted;

        public double TimestampExpires
        {
            get => _timestampExpires;
            set => _timestampExpires = value;
        }

        public bool HasExpired => (TimestampExpires - PlusEnvironment.GetUnixTimestamp()) < 0;

        public int MinutesLeft => Convert.ToInt32(Math.Ceiling((TimestampExpires - PlusEnvironment.GetUnixTimestamp()) / 60));

        public int CategoryId
        {
            get => _categoryId;
            set => _categoryId = value;
        }
    }
}