using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Outgoing.Groups
{
    class UpdateFavouriteGroupComposer : ServerPacket
    {
        public UpdateFavouriteGroupComposer(Group @group, int virtualId)
            : base(ServerPacketHeader.UpdateFavouriteGroupMessageComposer)
        {
            WriteInteger(virtualId);//Sends 0 on .COM
            WriteInteger(@group?.Id ?? 0);
            WriteInteger(3);
            WriteString(@group != null ? @group.Name : string.Empty);
        }
    }
}
