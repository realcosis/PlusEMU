namespace Plus.HabboHotel.Rooms.Chat.Filter
{
    sealed class WordFilter
    {
        private string _word;
        private string _replacement;
        private bool _strict;
        private bool _bannable;

        public WordFilter(string word, string replacement, bool strict, bool bannable)
        {
            _word = word;
            _replacement = replacement;
            _strict = strict;
            _bannable = bannable;
        }

        public string Word => _word;

        public string Replacement => _replacement;

        public bool IsStrict => _strict;

        public bool IsBannable => _bannable;
    }
}
