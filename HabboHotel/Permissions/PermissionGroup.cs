namespace Plus.HabboHotel.Permissions
{
    public class PermissionGroup
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Badge { get; set; }

        public PermissionGroup(string name, string description, string badge)
        {
            this.Name = name;
            this.Description = description;
            this.Badge = badge;
        }
    }
}
