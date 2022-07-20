using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks;

public class LoveLockDialogueSetLockedComposer : IServerPacket
{
    private readonly int _itemId;
    public uint MessageId => ServerPacketHeader.LoveLockDialogueSetLockedComposer;

    public LoveLockDialogueSetLockedComposer(int itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_itemId);
}