using System;
using System.Linq;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users.Relationships;
using Plus.Communication.Packets.Outgoing;

namespace Plus.HabboHotel.Users.Messenger
{
    public class MessengerBuddy
    {
        #region Fields

        public int UserId;
        public bool MAppearOffline;
        public bool MHideInroom;
        public int MLastOnline;
        public string MLook;
        public string MMotto;

        public GameClient Client;
        private Room _currentRoom;
        public string MUsername;

        #endregion

        #region Return values

        public int Id
        {
            get { return UserId; }
        }

        public bool IsOnline
        {
            get
            {
                return (Client != null && Client.GetHabbo() != null && Client.GetHabbo().GetMessenger() != null &&
                        !Client.GetHabbo().GetMessenger().AppearOffline);
            }
        }

        public bool InRoom
        {
            get { return (_currentRoom != null); }
        }

        public Room CurrentRoom
        {
            get { return _currentRoom; }
            set { _currentRoom = value; }
        }

        #endregion

        #region Constructor

        public MessengerBuddy(int userId, string pUsername, string pLook, string pMotto, int pLastOnline,
                                bool pAppearOffline, bool pHideInroom)
        {
            this.UserId = userId;
            MUsername = pUsername;
            MLook = pLook;
            MMotto = pMotto;
            MLastOnline = pLastOnline;
            MAppearOffline = pAppearOffline;
            MHideInroom = pHideInroom;
        }

        #endregion

        #region Methods
        public void UpdateUser(GameClient client)
        {
            this.Client = client;
            if (client != null && client.GetHabbo() != null)
                _currentRoom = client.GetHabbo().CurrentRoom;
        }

        public void Serialize(ServerPacket message, GameClient session)
        {
            Relationship relationship = null;

            if(session != null && session.GetHabbo() != null && session.GetHabbo().Relationships != null)
                relationship = session.GetHabbo().Relationships.FirstOrDefault(x => x.Value.UserId == Convert.ToInt32(UserId)).Value;

            var y = relationship == null ? 0 : relationship.Type;

            message.WriteInteger(UserId);
            message.WriteString(MUsername);
            message.WriteInteger(1);
            message.WriteBoolean(!MAppearOffline || session.GetHabbo().GetPermissions().HasRight("mod_tool") ? IsOnline : false);
            message.WriteBoolean(!MHideInroom || session.GetHabbo().GetPermissions().HasRight("mod_tool") ? InRoom : false);
            message.WriteString(IsOnline ? MLook : "");
            message.WriteInteger(0); // categoryid
            message.WriteString(MMotto);
            message.WriteString(string.Empty); // Facebook username
            message.WriteString(string.Empty);
            message.WriteBoolean(true); // Allows offline messaging
            message.WriteBoolean(false); // ?
            message.WriteBoolean(false); // Uses phone
            message.WriteShort(y);
        }

        #endregion
    }
}