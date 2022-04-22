namespace Plus.HabboHotel.Navigator
{
    public class SearchResultList
    {
        private int _id;
        private string _category;
        private string _categoryName;
        private string _customName;
        private bool _canDoActions;
        private int _colour;
        private int _requiredRank;
        private NavigatorViewMode _viewMode;
        private NavigatorCategoryType _categoryType;
        private NavigatorSearchAllowance _searchAllowance;
        private int _orderId;

        public SearchResultList(int id, string category, string categoryIdentifier, string publicName, bool canDoActions, int colour, int requiredRank, NavigatorViewMode viewMode, string categoryType, string searchAllowance, int orderId)
        {
            _id = id;
            _category = category;
            _categoryName = categoryIdentifier;
            _customName = publicName;
            _canDoActions = canDoActions;
            _colour = colour;
            _requiredRank = requiredRank;
            _viewMode = viewMode;
            _categoryType = NavigatorCategoryTypeUtility.GetCategoryTypeByString(categoryType);
            _searchAllowance = NavigatorSearchAllowanceUtility.GetSearchAllowanceByString(searchAllowance);
            _orderId = orderId;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        //TODO: Make an enum?
        public string Category
        {
            get => _category;
            set => _category = value;
        }

        public string CategoryIdentifier
        {
            get => _categoryName;
            set => _categoryName = value;
        }

        public string PublicName
        {
            get => _customName;
            set => _customName = value;
        }

        public bool CanDoActions
        {
            get => _canDoActions;
            set => _canDoActions = value;
        }

        public int Colour
        {
            get => _colour;
            set => _colour = value;
        }

        public int RequiredRank
        {
            get => _requiredRank;
            set => _requiredRank = value;
        }

        public NavigatorViewMode ViewMode
        {
            get => _viewMode;
            set => _viewMode = value;
        }

        public NavigatorCategoryType CategoryType
        {
            get => _categoryType;
            set => _categoryType = value;
        }

        public NavigatorSearchAllowance SearchAllowance
        {
            get => _searchAllowance;
            set => _searchAllowance = value;
        }

        public int OrderId
        {
            get => _orderId;
            set => _orderId = value;
        }
    }
}
