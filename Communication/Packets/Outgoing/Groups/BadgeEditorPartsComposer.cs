using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Outgoing.Groups;

internal class BadgeEditorPartsComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.BadgeEditorPartsMessageComposer;

    private readonly ICollection<GroupBadgeParts> _bases;
    private readonly ICollection<GroupBadgeParts> _symbols;
    private readonly ICollection<GroupColours> _baseColours;
    private readonly ICollection<GroupColours> _symbolColours;
    private readonly ICollection<GroupColours> _backgroundColours;

    public BadgeEditorPartsComposer(ICollection<GroupBadgeParts> bases, ICollection<GroupBadgeParts> symbols, ICollection<GroupColours> baseColours, ICollection<GroupColours> symbolColours, ICollection<GroupColours> backgroundColours)
    {
        _bases = bases;
        _symbols = symbols;
        _baseColours = baseColours;
        _symbolColours = symbolColours;
        _backgroundColours = backgroundColours;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_bases.Count);
        foreach (var part in _bases)
        {
            packet.WriteInteger(part.Id);
            packet.WriteString(part.AssetOne);
            packet.WriteString(part.AssetTwo);
        }
        packet.WriteInteger(_symbols.Count);
        foreach (var part in _symbols)
        {
            packet.WriteInteger(part.Id);
            packet.WriteString(part.AssetOne);
            packet.WriteString(part.AssetTwo);
        }
        packet.WriteInteger(_baseColours.Count);
        foreach (var color in _baseColours)
        {
            packet.WriteInteger(color.Id);
            packet.WriteString(color.Colour);
        }
        packet.WriteInteger(_symbolColours.Count);
        foreach (var color in _symbolColours)
        {
            packet.WriteInteger(color.Id);
            packet.WriteString(color.Colour);
        }
        packet.WriteInteger(_backgroundColours.Count);
        foreach (var color in _backgroundColours)
        {
            packet.WriteInteger(color.Id);
            packet.WriteString(color.Colour);
        }
    }
}