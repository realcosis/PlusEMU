using System;
using System.Collections.Generic;
using System.Data;
using NLog;
using Plus.Database;
using Plus.HabboHotel.Badges;

namespace Plus.HabboHotel.Badges;

public class BadgeManager : IBadgeManager
{
    private readonly IDatabase _database;
    private readonly IBadgeManager _badgeManager;

    public BadgeManager(IBadgeManager badgeManager, IDatabase database)
    {
        _database = database;
        _badgeManager = badgeManager;
        _badges = new Dictionary<string, BadgeDefinition>();
    }

    private static readonly ILogger Log = LogManager.GetLogger("Plus.HabboHotel.Badges.BadgeManager");

    private readonly Dictionary<string, BadgeDefinition> _badges;

    public void Init()
    {
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery("SELECT * FROM `badge_definitions`;");
            var data = dbClient.GetTable();
            foreach (DataRow row in data.Rows)
            {
                var code = Convert.ToString(row["code"]).ToUpper();
                if (!_badges.ContainsKey(code))
                    _badges.Add(code, new BadgeDefinition(code, Convert.ToString(row["required_right"])));
            }
        }
        Log.Info("Loaded " + _badges.Count + " badge definitions.");
    }

    public bool TryGetBadge(string code, out BadgeDefinition badge) => _badges.TryGetValue(code.ToUpper(), out badge);
}