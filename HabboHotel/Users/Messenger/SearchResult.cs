namespace Plus.HabboHotel.Users.Messenger
{
    public struct SearchResult
    {
        public int UserId;
        public string Username;
        public string Motto;
        public string Figure;
        public string LastOnline;

        public SearchResult(int userId, string username, string motto, string figure, string lastOnline)
        {
            this.UserId = userId;
            this.Username = username;
            this.Motto = motto;
            this.Figure = figure;
            this.LastOnline = lastOnline;
        }
    }
}