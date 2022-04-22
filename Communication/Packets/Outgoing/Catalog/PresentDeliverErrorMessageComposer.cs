namespace Plus.Communication.Packets.Outgoing.Catalog
{
    class PresentDeliverErrorMessageComposer : ServerPacket
    {
        public PresentDeliverErrorMessageComposer(bool creditError, bool ducketError)
            : base(ServerPacketHeader.PresentDeliverErrorMessageComposer)
        {
            WriteBoolean(creditError);//Do we have enough credits?
            WriteBoolean(ducketError);//Do we have enough duckets?
        }
    }
}
