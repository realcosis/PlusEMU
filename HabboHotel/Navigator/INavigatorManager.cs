using System.Collections.Generic;

namespace Plus.HabboHotel.Navigator;

public interface INavigatorManager
{
    void Init();
    List<SearchResultList> GetCategorysForSearch(string category);
    ICollection<SearchResultList> GetResultByIdentifier(string category);
    ICollection<SearchResultList> GetFlatCategories();
    ICollection<SearchResultList> GetEventCategories();
    ICollection<TopLevelItem> GetTopLevelItems();
    ICollection<SearchResultList> GetSearchResultLists();
    bool TryGetTopLevelItem(int id, out TopLevelItem topLevelItem);
    bool TryGetSearchResultList(int id, out SearchResultList searchResultList);
    bool TryGetFeaturedRoom(int roomId, out FeaturedRoom publicRoom);
    ICollection<FeaturedRoom> GetFeaturedRooms();
}