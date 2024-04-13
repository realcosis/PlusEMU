using Plus.Communication.Packets.Outgoing.Camera;
using Plus.Communication.Packets.Outgoing.Notifications;
using Plus.Core.Language;
using Plus.Core.Settings;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Camera;

internal class RenderRoomThumbnailEvent : IPacketEvent
{
    readonly ILanguageManager _languageManager;
    readonly ISettingsManager _settingsManager;

    public RenderRoomThumbnailEvent(ILanguageManager languageManager, ISettingsManager settingsManager)
    {
        _languageManager = languageManager;
        _settingsManager = settingsManager;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var room = session.GetHabbo().CurrentRoom;
        if (room == default)
            return;

        if (!room.CheckRights(session, true) && !session.GetHabbo().Permissions.HasRight("mod_tickets"))
            return;

        var bytes = packet.ReadBytes() ?? Array.Empty<byte>();
        if (bytes!.Length == 0)
            return;

        if (!IsPNG(bytes))
            return;

        try
        {
            var fileName = $"{room.Id}.png";
            var cameraPath = _settingsManager.TryGetValue("imager.location.output.thumbnail");
            await File.WriteAllBytesAsync($"{cameraPath}{fileName}", bytes);
            session.Send(new ThumbnailStatusComposer());
        }
        catch
        {
            session.Send(new MotdNotificationComposer(_languageManager.TryGetValue("camera.error.creation")));
        }
    }

    static bool IsPNG(byte[] bytes) => bytes[1] == 80 && bytes[2] == 78 && bytes[3] == 71;
}