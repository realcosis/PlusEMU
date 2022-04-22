using System;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni
{
    class UpdateMagicTileComposer : ServerPacket
    {
        public UpdateMagicTileComposer(int itemId, int @decimal)
            : base(ServerPacketHeader.UpdateMagicTileMessageComposer)
        {
            WriteInteger(Convert.ToInt32(itemId));
            WriteInteger(@decimal);
        }
    }
}
