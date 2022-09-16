using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Clothing;

namespace Plus.Communication.Packets.Outgoing.Avatar;

public class WardrobeComposer : IServerPacket
{
    public uint MessageId => ServerPacketHeader.WardrobeComposer;

    private readonly Wardrobe _wardrobe;
    public WardrobeComposer(Wardrobe wardrobe)
    {
        _wardrobe = wardrobe;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(1);
        packet.WriteInteger(_wardrobe.SavedLooks.Count);
        foreach (var look in _wardrobe.SavedLooks)
        {
            packet.WriteInteger(look.SlotId);
            packet.WriteString(look.Look);
            packet.WriteString(look.Gender.ToUpper());
        }
    }
}