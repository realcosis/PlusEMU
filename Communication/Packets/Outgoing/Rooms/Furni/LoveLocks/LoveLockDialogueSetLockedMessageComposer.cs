using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks;

internal class LoveLockDialogueSetLockedMessageComposer : IServerPacket
{
    private readonly int _itemId;
    public int MessageId => ServerPacketHeader.LoveLockDialogueSetLockedMessageComposer;

    public LoveLockDialogueSetLockedMessageComposer(int itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_itemId);
}