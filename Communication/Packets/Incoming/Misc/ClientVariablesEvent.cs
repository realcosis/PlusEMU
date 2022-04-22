using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Misc
{
    class ClientVariablesEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var gordanPath = packet.PopString();
            var externalVariables = packet.PopString();
        }
    }
}
