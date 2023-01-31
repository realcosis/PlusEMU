using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks;

public class LoveLockDialogueCloseComposer : IServerPacket
{
    private readonly uint _itemId;
    public uint MessageId => ServerPacketHeader.LoveLockDialogueCloseComposer;

    public LoveLockDialogueCloseComposer(uint itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteUInteger(_itemId);
}