using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Effects;

namespace Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects;

public class AvatarEffectsComposer : IServerPacket
{
    private readonly ICollection<AvatarEffect> _effects;

    public uint MessageId => ServerPacketHeader.AvatarEffectsComposer;

    public AvatarEffectsComposer(ICollection<AvatarEffect> effects)
    {
        _effects = effects;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_effects.Count);
        foreach (var effect in _effects)
        {
            packet.WriteInteger(effect.SpriteId); //Effect Id
            packet.WriteInteger(0); //Type, 0 = Hand, 1 = Full
            packet.WriteInteger((int)effect.Duration);
            packet.WriteInteger(effect.Activated ? effect.Quantity - 1 : effect.Quantity);
            packet.WriteInteger(effect.Activated ? (int)effect.TimeLeft : -1);
            packet.WriteBoolean(false); //Permanent
        }

    }
}