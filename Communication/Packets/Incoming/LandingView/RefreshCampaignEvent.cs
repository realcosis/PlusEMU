﻿using Plus.Communication.Packets.Outgoing.LandingView;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.LandingView;

internal class RefreshCampaignEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        try
        {
            var parseCampaings = packet.ReadString();
            if (parseCampaings.Contains("gamesmaker"))
                return Task.CompletedTask;
            var campaingName = "";
            var parser = parseCampaings.Split(';');
            foreach (var value in parser)
            {
                if (string.IsNullOrEmpty(value) || value.EndsWith(","))
                    continue;
                var data = value.Split(',');
                campaingName = data[1];
            }
            session.Send(new CampaignComposer(parseCampaings, campaingName));
        }
        catch
        {
            //ignored
        }
        return Task.CompletedTask;
    }
}