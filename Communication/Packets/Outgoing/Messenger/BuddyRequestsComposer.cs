using System.Collections.Generic;

using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.Messenger
{
    class BuddyRequestsComposer : ServerPacket
    {
        public BuddyRequestsComposer(ICollection<MessengerRequest> requests)
            : base(ServerPacketHeader.BuddyRequestsMessageComposer)
        {
            WriteInteger(requests.Count);
            WriteInteger(requests.Count);

            foreach (var request in requests)
            {
                WriteInteger(request.From);
                WriteString(request.Username);

                var user = PlusEnvironment.GetGame().GetCacheManager().GenerateUser(request.From);
                WriteString(user != null ? user.Look : "");
            }
        }
    }
}
