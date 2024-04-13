using Plus.Communication.Packets.Outgoing.Camera;
using Plus.Communication.Packets.Outgoing.Inventory.Furni;
using Plus.Communication.Packets.Outgoing.Notifications;
using Plus.Core.Language;
using Plus.Core.Settings;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Incoming.Camera;

internal class PurchasePhotoEvent : IPacketEvent
{
    readonly IItemFactory _itemFactory;
    readonly IItemDataManager _itemDataManager;
    readonly ISettingsManager _settingsManager;
    readonly ILanguageManager _languageManager;

    public PurchasePhotoEvent(ISettingsManager settingsManager, ILanguageManager languageManager,
                              IItemFactory itemFactory, IItemDataManager itemDataManager)
    {
        _itemFactory = itemFactory;
        _itemDataManager = itemDataManager;
        _settingsManager = settingsManager;
        _languageManager = languageManager;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        if (!int.TryParse(_settingsManager.TryGetValue("camera.price.points.purchase"), out var cameraPrice))
            return Task.CompletedTask;

        if (!int.TryParse(_settingsManager.TryGetValue("camera.price.points.purchase.type"), out var pointsType))
            return Task.CompletedTask;

        if (!uint.TryParse(_settingsManager.TryGetValue("camera.item.id"), out var itemID))
            return Task.CompletedTask;

        var userCurrencyValue = pointsType switch
        {
            -1 => session.GetHabbo().Credits,
            0 => session.GetHabbo().Duckets,
            5 => session.GetHabbo().Diamonds,
            _ => session.GetHabbo().Credits
        };
        var userCurrencyName = _languageManager.TryGetValue($"user.currency.name.{pointsType}") ?? _languageManager.TryGetValue($"user.currency.name.-1");

        if (userCurrencyValue < cameraPrice)
        {
            var message = _languageManager.TryGetValue("camera.points.not_enough");
            session.Send(new MotdNotificationComposer(string.Format(message, userCurrencyName)));
            return Task.CompletedTask;
        }

        if (_itemDataManager.Items.TryGetValue(itemID, out var definition))
        {
            var extraData = session.GetHabbo().Photo;
            if (!string.IsNullOrWhiteSpace(extraData))
            {
                var item = _itemFactory.CreateSingleItemNullable(definition, session.GetHabbo(), extraData, extraData);
                if (item != default)
                {
                    session.GetHabbo().Inventory.Furniture.AddItem(item.ToInventoryItem());
                    session.Send(new CameraPurchaseOKComposer());
                    session.Send(new FurniListNotificationComposer(item.Id, 1));
                    session.Send(new FurniListUpdateComposer());
                    session.GetHabbo().Give(-cameraPrice, pointsType);
                }
            }
        }
        return Task.CompletedTask;
    }
}