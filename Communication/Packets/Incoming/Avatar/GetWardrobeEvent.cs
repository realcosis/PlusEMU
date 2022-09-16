using Plus.Communication.Packets.Outgoing.Avatar;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Clothing;

namespace Plus.Communication.Packets.Incoming.Avatar;

internal class GetWardrobeEvent : IPacketEvent
{
    private readonly IWardrobeLoader _wardrobeLoader;

    public GetWardrobeEvent(IWardrobeLoader wardrobeLoader)
    {
        _wardrobeLoader = wardrobeLoader;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var wardrobe = await _wardrobeLoader.LoadUserWardrobe(session.GetHabbo().Id);
        if (wardrobe == null) return;
        session.Send(new WardrobeComposer(wardrobe));
    }
}