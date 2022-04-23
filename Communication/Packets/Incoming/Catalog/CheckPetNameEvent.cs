using Plus.Communication.Packets.Outgoing.Catalog;
using Plus.HabboHotel.GameClients;
using Plus.Utilities;

namespace Plus.Communication.Packets.Incoming.Catalog;

public class CheckPetNameEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var petName = packet.PopString();
        if (string.IsNullOrWhiteSpace(petName) || petName.Length < 2)
        {
            session.SendPacket(new CheckPetNameComposer(2, "2"));
            return;
        }
        if (petName.Length > 15)
        {
            session.SendPacket(new CheckPetNameComposer(1, "15"));
            return;
        }
        if (!StringCharFilter.IsValidAlphaNumeric(petName))
        {
            session.SendPacket(new CheckPetNameComposer(3, string.Empty));
            return;
        }
        if (PlusEnvironment.GetGame().GetChatManager().GetFilter().IsFiltered(petName))
        {
            session.SendPacket(new CheckPetNameComposer(4, string.Empty));
            return;
        }
        session.SendPacket(new CheckPetNameComposer(0, string.Empty));
    }
}