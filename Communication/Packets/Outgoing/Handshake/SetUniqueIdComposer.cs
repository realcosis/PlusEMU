namespace Plus.Communication.Packets.Outgoing.Handshake
{
    class SetUniqueIdComposer : ServerPacket
    {
        public SetUniqueIdComposer(string id)
            : base(ServerPacketHeader.SetUniqueIdMessageComposer)
        {
           WriteString(id);
        }
    }
}
