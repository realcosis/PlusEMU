using System;
using System.Linq;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Relationships;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.Messenger
{
    class FriendListUpdateComposer : ServerPacket
    {
        public FriendListUpdateComposer(int friendId)
            : base(ServerPacketHeader.FriendListUpdateMessageComposer)
        {
            WriteInteger(0);//Category Count
            WriteInteger(1);//Updates Count
            WriteInteger(-1);//Update
            WriteInteger(friendId);
        }

        public FriendListUpdateComposer(GameClient session, MessengerBuddy buddy)
            : base(ServerPacketHeader.FriendListUpdateMessageComposer)
        {
            WriteInteger(0);//Category Count
            WriteInteger(1);//Updates Count
            WriteInteger(0);//Update

            Relationship relationship = session.GetHabbo().Relationships.FirstOrDefault(x => x.Value.UserId == Convert.ToInt32(buddy.UserId)).Value;
            int y = relationship == null ? 0 : relationship.Type;

            WriteInteger(buddy.UserId);
           WriteString(buddy.MUsername);
            WriteInteger(1);
            if (!buddy.MAppearOffline || session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                WriteBoolean(buddy.IsOnline);
            else
                WriteBoolean(false);

            if (!buddy.MHideInroom || session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                WriteBoolean(buddy.InRoom);
            else
                WriteBoolean(false);

           WriteString("");//Habbo.IsOnline ? Habbo.Look : "");
            WriteInteger(0); // categoryid
           WriteString(buddy.MMotto);
           WriteString(string.Empty); // Facebook username
           WriteString(string.Empty);
            WriteBoolean(true); // Allows offline messaging
            WriteBoolean(false); // ?
            WriteBoolean(false); // Uses phone
            WriteShort(y);
        }
    }
}
