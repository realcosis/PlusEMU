using Plus.Communication.Packets.Outgoing.Camera;
using Plus.Communication.Packets.Outgoing.Notifications;
using Plus.Core.Language;
using Plus.Core.Settings;
using Plus.HabboHotel.GameClients;
using Plus.Utilities;
using SkiaSharp;

namespace Plus.Communication.Packets.Incoming.Camera;

internal class RenderRoomEvent : IPacketEvent
{
    readonly ILanguageManager _languageManager;
    readonly ISettingsManager _settingsManager;

    public RenderRoomEvent(ILanguageManager languageManager, ISettingsManager settingsManager)
    {
        _languageManager = languageManager;
        _settingsManager = settingsManager;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var room = session.GetHabbo().CurrentRoom;
        if (room == default)
            return;

        var bytes = packet.ReadBytes() ?? Array.Empty<byte>();
        if (bytes!.Length == 0)
            return;

        if (!IsPNG(bytes))
            return;

        try
        {
            var imageId = Guid.NewGuid();
            var fileName = $"{imageId}.png";
            var url = $"{_settingsManager.TryGetValue("camera.url")}{fileName}";
            session.GetHabbo().Photo = $"{{\"t\":{UnixTimestamp.GetNow()}, \"w\":\"{url}\", \"f\":\"{imageId}\", \"n\":\"{session.GetHabbo().Username}\"}}";
            var cameraPath = _settingsManager.TryGetValue("imager.location.output.camera");
            using var image = SKBitmap.Decode(bytes);
            using var newImage = SKImage.FromBitmap(image.Resize(new SKImageInfo(100, 100), SKFilterQuality.High));
            await File.WriteAllBytesAsync($"{cameraPath}{fileName}", bytes);
            await File.WriteAllBytesAsync($"{cameraPath}{imageId}_small.png", newImage.Encode().ToArray());
            session.Send(new CameraStorageUrlComposer(fileName));
        }
        catch (Exception)
        {
            session.Send(new MotdNotificationComposer(_languageManager.TryGetValue("camera.error.creation")));
        }
    }

    static bool IsPNG(byte[] bytes) => bytes[1] == 80 && bytes[2] == 78 && bytes[3] == 71;
}