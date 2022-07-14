using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

internal class RoomSettingsDataComposer : IServerPacket
{
    private readonly Room _room;

    public int MessageId => ServerPacketHeader.RoomSettingsDataMessageComposer;

    public RoomSettingsDataComposer(Room room)
    {
        _room = room;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_room.RoomId);
        packet.WriteString(_room.Name);
        packet.WriteString(_room.Description);
        packet.WriteInteger(RoomAccessUtility.GetRoomAccessPacketNum(_room.Access));
        packet.WriteInteger(_room.Category);
        packet.WriteInteger(_room.UsersMax);
        packet.WriteInteger(_room.Model.MapSizeX * _room.Model.MapSizeY > 100 ? 50 : 25);
        packet.WriteInteger(_room.Tags.Count);
        foreach (var tag in _room.Tags.ToArray()) packet.WriteString(tag);
        packet.WriteInteger(_room.TradeSettings); //Trade
        packet.WriteInteger(_room.AllowPets); // allows pets in room - pet system lacking, so always off
        packet.WriteInteger(_room.AllowPetsEating); // allows pets to eat your food - pet system lacking, so always off
        packet.WriteInteger(_room.RoomBlockingEnabled);
        packet.WriteInteger(_room.Hidewall);
        packet.WriteInteger(_room.WallThickness);
        packet.WriteInteger(_room.FloorThickness);
        packet.WriteInteger(_room.ChatMode); //Chat mode
        packet.WriteInteger(_room.ChatSize); //Chat size
        packet.WriteInteger(_room.ChatSpeed); //Chat speed
        packet.WriteInteger(_room.ChatDistance); //Hearing Distance
        packet.WriteInteger(_room.ExtraFlood); //Additional Flood
        packet.WriteBoolean(true);
        packet.WriteInteger(_room.WhoCanMute); // who can mute
        packet.WriteInteger(_room.WhoCanKick); // who can kick
        packet.WriteInteger(_room.WhoCanBan); // who can ban

    }
}