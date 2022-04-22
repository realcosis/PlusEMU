using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Plus.HabboHotel.Users.Navigator.SavedSearches;

public class SearchesComponent
{
    private readonly ConcurrentDictionary<int, SavedSearch> _savedSearches;

    public SearchesComponent()
    {
        _savedSearches = new ConcurrentDictionary<int, SavedSearch>();
    }

    public ICollection<SavedSearch> Searches => _savedSearches.Values;

    public bool Init(Habbo habbo)
    {
        if (_savedSearches.Count > 0)
            _savedSearches.Clear();
        DataTable getSearches = null;
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("SELECT `id`,`filter`,`search_code` FROM `user_saved_searches` WHERE `user_id` = @UserId");
        dbClient.AddParameter("UserId", habbo.Id);
        getSearches = dbClient.GetTable();
        if (getSearches != null)
        {
            foreach (DataRow row in getSearches.Rows)
                _savedSearches.TryAdd(Convert.ToInt32(row["id"]), new SavedSearch(Convert.ToInt32(row["id"]), Convert.ToString(row["filter"]), Convert.ToString(row["search_code"])));
        }
        return true;
    }

    public bool TryAdd(int id, SavedSearch search) => _savedSearches.TryAdd(id, search);

    public bool TryRemove(int id, out SavedSearch removed) => _savedSearches.TryRemove(id, out removed);
}