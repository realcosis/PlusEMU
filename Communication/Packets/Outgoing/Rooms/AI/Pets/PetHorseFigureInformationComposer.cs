using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.AI.Pets;

public class PetHorseFigureInformationComposer : IServerPacket
{
    private readonly RoomUser _petUser;
    public uint MessageId => ServerPacketHeader.PetHorseFigureInformationComposer;

    public PetHorseFigureInformationComposer(RoomUser petUser)
    {
        _petUser = petUser;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_petUser.PetData.VirtualId);
        packet.WriteInteger(_petUser.PetData.PetId);
        packet.WriteInteger(_petUser.PetData.Type);
        packet.WriteInteger(int.Parse(_petUser.PetData.Race));
        packet.WriteString(_petUser.PetData.Color.ToLower());
        if (_petUser.PetData.Saddle > 0)
        {
            packet.WriteInteger(4);
            packet.WriteInteger(3);
            packet.WriteInteger(3);
            packet.WriteInteger(_petUser.PetData.PetHair);
            packet.WriteInteger(_petUser.PetData.HairDye);
            packet.WriteInteger(2);
            packet.WriteInteger(_petUser.PetData.PetHair);
            packet.WriteInteger(_petUser.PetData.HairDye);
            packet.WriteInteger(4);
            packet.WriteInteger(_petUser.PetData.Saddle);
            packet.WriteInteger(0);
        }
        else
        {
            packet.WriteInteger(1);
            packet.WriteInteger(2);
            packet.WriteInteger(2);
            packet.WriteInteger(_petUser.PetData.PetHair);
            packet.WriteInteger(_petUser.PetData.HairDye);
            packet.WriteInteger(3);
            packet.WriteInteger(_petUser.PetData.PetHair);
            packet.WriteInteger(_petUser.PetData.HairDye);
        }
        packet.WriteBoolean(_petUser.PetData.Saddle > 0);
        packet.WriteBoolean(_petUser.RidingHorse);
    }
}