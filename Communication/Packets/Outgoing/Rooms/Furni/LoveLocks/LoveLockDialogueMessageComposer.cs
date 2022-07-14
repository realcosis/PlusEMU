using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks;

internal class LoveLockDialogueMessageComposer : IServerPacket
{
    private readonly int _itemId;

    public int MessageId => ServerPacketHeader.LoveLockDialogueMessageComposer;

    public LoveLockDialogueMessageComposer(int itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_itemId);
        packet.WriteBoolean(true);
    }
}