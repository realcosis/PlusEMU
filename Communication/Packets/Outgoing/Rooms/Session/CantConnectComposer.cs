namespace Plus.Communication.Packets.Outgoing.Rooms.Session
{
    class CantConnectComposer : ServerPacket
    {
        public CantConnectComposer(int error)
            : base(ServerPacketHeader.CantConnectMessageComposer)
        {
            WriteInteger(error);
        }
    }
}
