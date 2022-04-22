using System.Collections.Generic;

using Plus.HabboHotel.Users.Effects;

namespace Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
    class AvatarEffectsComposer : ServerPacket
    {
        public AvatarEffectsComposer(ICollection<AvatarEffect> effects)
            : base(ServerPacketHeader.AvatarEffectsMessageComposer)
        {
            WriteInteger(effects.Count);

            foreach (AvatarEffect effect in effects)
            {
                WriteInteger(effect.SpriteId);//Effect Id
                WriteInteger(0);//Type, 0 = Hand, 1 = Full
                WriteInteger((int)effect.Duration);
                WriteInteger(effect.Activated ? effect.Quantity - 1 : effect.Quantity);
                WriteInteger(effect.Activated ? (int)effect.TimeLeft : -1);
                WriteBoolean(false);//Permanent
            }
        }
    }
}
