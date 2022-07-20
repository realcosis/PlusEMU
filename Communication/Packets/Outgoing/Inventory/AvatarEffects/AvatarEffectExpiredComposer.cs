using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Effects;

namespace Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects;

public class AvatarEffectExpiredComposer : IServerPacket
{
    private readonly AvatarEffect _effect;
    public uint MessageId => ServerPacketHeader.AvatarEffectExpiredComposer;

    public AvatarEffectExpiredComposer(AvatarEffect effect)
    {
        _effect = effect;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_effect.SpriteId);
}