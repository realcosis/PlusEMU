using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks;

public class LoveLockDialogueSetLockedComposer : IServerPacket
{
    private readonly uint _itemId;
    public uint MessageId => ServerPacketHeader.LoveLockDialogueSetLockedComposer;

    public LoveLockDialogueSetLockedComposer(uint itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteUInteger(_itemId);
}