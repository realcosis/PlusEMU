using System.Collections.Generic;

namespace Plus.HabboHotel.Catalog
{
    public class CatalogPage
    {
        private int _id;
        private int _parentId;
        private string _caption;
        private string _pageLink;
        private int _icon;
        private int _minRank;
        private int _minVip;
        private bool _visible;
        private bool _enabled;
        private string _template;

        private List<string> _pageStrings1;
        private List<string> _pageStrings2;

        private Dictionary<int, CatalogItem> _items;
        private Dictionary<int, CatalogItem> _itemOffers;

        public CatalogPage(int id, int parentId, string enabled, string caption, string pageLink, int icon, int minRank, int minVip,
              string visible, string template, string pageStrings1, string pageStrings2, Dictionary<int, CatalogItem> items, ref Dictionary<int, int> flatOffers)
        {
            _id = id;
            _parentId = parentId;
            _enabled = enabled.ToLower() == "1" ? true : false;
            _caption = caption;
            _pageLink = pageLink;
            _icon = icon;
            _minRank = minRank;
            _minVip = minVip;
            _visible = visible.ToLower() == "1" ? true : false;
            _template = template;

            foreach (var str in pageStrings1.Split('|'))
            {
                if (_pageStrings1 == null) { _pageStrings1 = new List<string>(); }
                _pageStrings1.Add(str);
            }

            foreach (var str in pageStrings2.Split('|'))
            {
                if (_pageStrings2 == null) { _pageStrings2 = new List<string>(); }
                _pageStrings2.Add(str);
            }

            _items = items;

            _itemOffers = new Dictionary<int, CatalogItem>();
            foreach (var i in flatOffers.Keys)
            {
                if (flatOffers[i] == id)
                {
                    foreach (var item in _items.Values)
                    {
                        if (item.OfferId == i)
                        {
                            if (!_itemOffers.ContainsKey(i))
                                _itemOffers.Add(i, item);
                        }
                    }
                }
            }
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public int ParentId
        {
            get => _parentId;
            set => _parentId = value;
        }

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public string Caption
        {
            get => _caption;
            set => _caption = value;
        }

        public string PageLink
        {
            get => _pageLink;
            set => _pageLink = value;
        }

        public int Icon
        {
            get => _icon;
            set => _icon = value;
        }

        public int MinimumRank
        {
            get => _minRank;
            set => _minRank = value;
        }

        public int MinimumVip
        {
            get => _minVip;
            set => _minVip = value;
        }

        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        public string Template
        {
            get => _template;
            set => _template = value;
        }

        public List<string> PageStrings1
        {
            get => _pageStrings1;
            private set => _pageStrings1 = value;
        }

        public List<string> PageStrings2
        {
            get => _pageStrings2;
            private set => _pageStrings2 = value;
        }

        public Dictionary<int, CatalogItem> Items
        {
            get => _items;
            private set => _items = value;
        }
        
        public Dictionary<int, CatalogItem> ItemOffers
        {
            get => _itemOffers;
            private set => _itemOffers = value;
        }

        public CatalogItem GetItem(int pId)
        {
            if (_items.ContainsKey(pId))
                return (CatalogItem)_items[pId];
            return null;
        }
    }
}