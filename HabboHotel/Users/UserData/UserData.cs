using System.Collections.Concurrent;
using System.Collections.Generic;
using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.Users.Badges;
using Plus.HabboHotel.Users.Messenger;
using Plus.HabboHotel.Users.Relationships;

namespace Plus.HabboHotel.Users.UserData
{
    public class UserData
    {
        public int UserId { get; private set; }
        public Habbo User;

        public Dictionary<int, Relationship> Relations;
        public ConcurrentDictionary<string, UserAchievement> Achievements;
        public List<Badge> Badges;
        public List<int> FavouritedRooms;
        public Dictionary<int, MessengerRequest> Requests;
        public Dictionary<int, MessengerBuddy> Friends;
        public Dictionary<int, int> Quests;

        public UserData(int userId, ConcurrentDictionary<string, UserAchievement> achievements, List<int> favouritedRooms,
            List<Badge> badges, Dictionary<int, MessengerBuddy> friends, Dictionary<int, MessengerRequest> requests, Dictionary<int, int> quests, Habbo user, 
            Dictionary<int, Relationship> relations)
        {
            UserId = userId;
            this.Achievements = achievements;
            this.FavouritedRooms = favouritedRooms;
            this.Badges = badges;
            this.Friends = friends;
            this.Requests = requests;
            this.Quests = quests;
            this.User = user;
            this.Relations = relations;
        }
    }
}