using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks;

public class LoveLockDialogueComposer : IServerPacket
{
    private readonly uint _itemId;

    public uint MessageId => ServerPacketHeader.LoveLockDialogueComposer;

    public LoveLockDialogueComposer(uint itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteUInteger(_itemId);
        packet.WriteBoolean(true);
    }
}