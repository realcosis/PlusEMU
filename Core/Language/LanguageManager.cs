using System.Collections.Generic;
using System.Data;
using NLog;

namespace Plus.Core.Language;

public class LanguageManager
{
    private static readonly ILogger Log = LogManager.GetLogger("Plus.Core.Language.LanguageManager");
    private readonly Dictionary<string, string> _values;

    public LanguageManager()
    {
        _values = new Dictionary<string, string>();
    }

    public void Init()
    {
        if (_values.Count > 0)
            _values.Clear();
        using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            dbClient.SetQuery("SELECT * FROM `server_locale`");
            var table = dbClient.GetTable();
            if (table != null)
                foreach (DataRow row in table.Rows)
                    _values.Add(row["key"].ToString(), row["value"].ToString());
        }
        Log.Info("Loaded " + _values.Count + " language locales.");
    }

    public string TryGetValue(string value) => _values.ContainsKey(value) ? _values[value] : "No language locale found for [" + value + "]";
}