using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Effects;

namespace Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects;

public class AvatarEffectActivatedComposer : IServerPacket
{
    private readonly AvatarEffect _effect;
    public uint MessageId => ServerPacketHeader.AvatarEffectActivatedComposer;

    public AvatarEffectActivatedComposer(AvatarEffect effect)
    {
        _effect = effect;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_effect.SpriteId);
        packet.WriteInteger((int)_effect.Duration);
        packet.WriteBoolean(false); //Permanent
    }
}