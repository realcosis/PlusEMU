using System.Data;
using Plus.Communication.Packets.Outgoing.Catalog;
using Plus.Communication.Packets.Outgoing.Inventory.Purse;
using Plus.Database;
using Plus.HabboHotel.Catalog.Vouchers;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Catalog;

public class RedeemVoucherEvent : IPacketEvent
{
    private readonly IVoucherManager _voucherManager;
    private readonly IDatabase _database;

    public RedeemVoucherEvent(IVoucherManager voucherManager, IDatabase database)
    {
        _voucherManager = voucherManager;
        _database = database;
    }
    public void Parse(GameClient session, ClientPacket packet)
    {
        var code = packet.PopString().Replace("\r", "");
        if (!_voucherManager.TryGetVoucher(code, out var voucher))
        {
            session.SendPacket(new VoucherRedeemErrorComposer(0));
            return;
        }
        if (voucher.CurrentUses >= voucher.MaxUses)
        {
            session.SendNotification("Oops, this voucher has reached the maximum usage limit!");
            return;
        }
        DataRow row;
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery("SELECT * FROM `user_vouchers` WHERE `user_id` = @userId AND `voucher` = @Voucher LIMIT 1");
            dbClient.AddParameter("userId", session.GetHabbo().Id);
            dbClient.AddParameter("Voucher", code);
            row = dbClient.GetRow();
        }
        if (row != null)
        {
            session.SendNotification("You've already used this voucher code, one per each user, sorry!");
            return;
        }
        {
            using var dbClient = _database.GetQueryReactor();
            dbClient.SetQuery("INSERT INTO `user_vouchers` (`user_id`,`voucher`) VALUES (@userId, @Voucher)");
            dbClient.AddParameter("userId", session.GetHabbo().Id);
            dbClient.AddParameter("Voucher", code);
            dbClient.RunQuery();
        }
        voucher.UpdateUses();
        if (voucher.Type == VoucherType.Credit)
        {
            session.GetHabbo().Credits += voucher.Value;
            session.SendPacket(new CreditBalanceComposer(session.GetHabbo().Credits));
        }
        else if (voucher.Type == VoucherType.Ducket)
        {
            session.GetHabbo().Duckets += voucher.Value;
            session.SendPacket(new HabboActivityPointNotificationComposer(session.GetHabbo().Duckets, voucher.Value));
        }
        session.SendPacket(new VoucherRedeemOkComposer());
    }
}