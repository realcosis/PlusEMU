namespace Plus.HabboHotel.Users.Messenger
{
    public class MessengerRequest
    {
        private int _toUser;
        private int _fromUser;
        private string _username;

        public MessengerRequest(int toUser, int fromUser, string username)
        {
            _toUser = toUser;
            _fromUser = fromUser;
            _username = username;
        }

        public string Username
        {
            get { return _username; }
        }

        public int To
        {
            get { return _toUser; }
        }

        public int From
        {
            get { return _fromUser; }
        }
    }
}