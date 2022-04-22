namespace Plus.HabboHotel.Items.Televisions
{
    public class TelevisionItem
    {
        private int _id;
        private string _youtubeId;
        private string _title;
        private string _description;
        private bool _enabled;

        public TelevisionItem(int id, string youTubeId, string title, string description, bool enabled)
        {
            _id = id;
            _youtubeId = youTubeId;
            _title = title;
            _description = description;
            _enabled = enabled;
        }

        public int Id => _id;

        public string YouTubeId => _youtubeId;


        public string Title => _title;

        public string Description => _description;

        public bool Enabled => _enabled;
    }
}
