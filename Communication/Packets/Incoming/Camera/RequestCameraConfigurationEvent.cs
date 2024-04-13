using Plus.Communication.Packets.Outgoing.Camera;
using Plus.Core.Settings;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Camera;

internal class RequestCameraConfigurationEvent : IPacketEvent
{
    readonly ISettingsManager _settingsManager;

    public RequestCameraConfigurationEvent(ISettingsManager settingsManager)
        => _settingsManager = settingsManager;

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        if (!int.TryParse(_settingsManager.TryGetValue("camera.price.points.purchase"), out var cameraPrice))
            return Task.CompletedTask;

        if (!int.TryParse(_settingsManager.TryGetValue("camera.price.points.publish"), out var photoPrice))
            return Task.CompletedTask;

        session.Send(new InitCameraComposer(cameraPrice, 0, photoPrice));
        return Task.CompletedTask;
    }
}