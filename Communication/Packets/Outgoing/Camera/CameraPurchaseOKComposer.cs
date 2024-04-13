﻿using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Camera;

internal class CameraPurchaseOKComposer : IServerPacket
{
    public uint MessageId => ServerPacketHeader.CameraPurchaseOKComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}