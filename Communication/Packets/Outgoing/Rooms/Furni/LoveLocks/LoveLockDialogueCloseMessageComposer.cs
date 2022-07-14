using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks;

internal class LoveLockDialogueCloseMessageComposer : IServerPacket
{
    private readonly int _itemId;
    public int MessageId => ServerPacketHeader.LoveLockDialogueCloseMessageComposer;

    public LoveLockDialogueCloseMessageComposer(int itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_itemId);
}