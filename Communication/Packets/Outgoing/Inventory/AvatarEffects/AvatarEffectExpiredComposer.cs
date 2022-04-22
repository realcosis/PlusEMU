using Plus.HabboHotel.Users.Effects;

namespace Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
    class AvatarEffectExpiredComposer : ServerPacket
    {
        public AvatarEffectExpiredComposer(AvatarEffect effect)
            : base(ServerPacketHeader.AvatarEffectExpiredMessageComposer)
        {
            WriteInteger(effect.SpriteId);
        }
    }
}
