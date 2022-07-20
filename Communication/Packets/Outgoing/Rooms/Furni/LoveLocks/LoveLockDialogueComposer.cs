using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks;

public class LoveLockDialogueComposer : IServerPacket
{
    private readonly int _itemId;

    public uint MessageId => ServerPacketHeader.LoveLockDialogueComposer;

    public LoveLockDialogueComposer(int itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_itemId);
        packet.WriteBoolean(true);
    }
}