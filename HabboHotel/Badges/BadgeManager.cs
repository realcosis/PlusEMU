using System;
using System.Collections.Generic;
using System.Data;
using NLog;

namespace Plus.HabboHotel.Badges;

public class BadgeManager : IBadgeManager
{
    private static readonly ILogger Log = LogManager.GetLogger("Plus.HabboHotel.Badges.BadgeManager");

    private readonly Dictionary<string, BadgeDefinition> _badges;

    public BadgeManager()
    {
        _badges = new Dictionary<string, BadgeDefinition>();
    }

    public void Init()
    {
        using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
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