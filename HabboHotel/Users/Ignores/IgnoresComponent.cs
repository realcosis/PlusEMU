using System;
using System.Collections.Generic;
using System.Data;

namespace Plus.HabboHotel.Users.Ignores;

public sealed class IgnoresComponent
{
    private readonly List<int> _ignoredUsers;

    public IgnoresComponent()
    {
        _ignoredUsers = new List<int>();
    }

    public bool Init(Habbo player)
    {
        if (_ignoredUsers.Count > 0)
            return false;
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("SELECT * FROM `user_ignores` WHERE `user_id` = @uid;");
        dbClient.AddParameter("uid", player.Id);
        var getIgnores = dbClient.GetTable();
        if (getIgnores != null)
            foreach (DataRow row in getIgnores.Rows)
                _ignoredUsers.Add(Convert.ToInt32(row["ignore_id"]));
        return true;
    }

    public bool TryGet(int userId) => _ignoredUsers.Contains(userId);

    public bool TryAdd(int userId)
    {
        if (_ignoredUsers.Contains(userId))
            return false;
        _ignoredUsers.Add(userId);
        return true;
    }

    public bool TryRemove(int userId) => _ignoredUsers.Remove(userId);

    public ICollection<int> IgnoredUserIds() => _ignoredUsers;

    public void Dispose()
    {
        _ignoredUsers.Clear();
    }
}