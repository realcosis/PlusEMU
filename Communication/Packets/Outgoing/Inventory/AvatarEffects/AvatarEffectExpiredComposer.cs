using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Effects;

namespace Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects;

internal class AvatarEffectExpiredComposer : IServerPacket
{
    private readonly AvatarEffect _effect;
    public int MessageId => ServerPacketHeader.AvatarEffectExpiredMessageComposer;

    public AvatarEffectExpiredComposer(AvatarEffect effect)
    {
        _effect = effect;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_effect.SpriteId);
}