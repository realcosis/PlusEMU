using System;
using System.Linq;
using System.Text;


using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.GameClients;


namespace Plus.Communication.Packets.Outgoing.Rooms.Engine
{
    class AvatarAspectUpdateMessageComposer : ServerPacket
    {
        public AvatarAspectUpdateMessageComposer(string figure, string gender)
            : base(ServerPacketHeader.AvatarAspectUpdateMessageComposer)
        {
            base.WriteString(figure);
            base.WriteString(gender);


        }
    }
}