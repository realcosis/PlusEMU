using System;
using System.Data;
using System.Collections.Generic;

namespace Plus.HabboHotel.Talents
{
    public class TalentTrackLevel
    {
        public string Type { get; set; }
        public int Level { get; set; }

        private List<string> _dataActions;
        private List<string> _dataGifts;

        private Dictionary<int, TalentTrackSubLevel> _subLevels;

        public TalentTrackLevel(string type, int level, string dataActions, string dataGifts)
        {
            this.Type = type;
            this.Level = level;

            foreach (var str in dataActions.Split('|'))
            {
                if (_dataActions == null) { _dataActions = new List<string>(); }
                _dataActions.Add(str);
            }

            foreach (var str in dataGifts.Split('|'))
            {
                if (_dataGifts == null) { _dataGifts = new List<string>(); }
                _dataGifts.Add(str);
            }

            _subLevels = new Dictionary<int, TalentTrackSubLevel>();

            Init();
        }

        public List<string> Actions
        {
            get { return _dataActions; }
            private set { _dataActions = value; }
        }

        public List<string> Gifts
        {
            get { return _dataGifts; }
            private set { _dataGifts = value; }
        }

        public void Init()
        {
            DataTable getTable = null;
            using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `sub_level`,`badge_code`,`required_progress` FROM `talents_sub_levels` WHERE `talent_level` = @TalentLevel");
                dbClient.AddParameter("TalentLevel", Level);
                getTable = dbClient.GetTable();
            }

            if (getTable != null)
            {
                foreach (DataRow row in getTable.Rows)
                {
                    _subLevels.Add(Convert.ToInt32(row["sub_level"]), new TalentTrackSubLevel(Convert.ToInt32(row["sub_level"]), Convert.ToString(row["badge_code"]), Convert.ToInt32(row["required_progress"])));
                }
            }
        }

        public ICollection<TalentTrackSubLevel> GetSubLevels()
        {
            return _subLevels.Values;
        }
    }
}
