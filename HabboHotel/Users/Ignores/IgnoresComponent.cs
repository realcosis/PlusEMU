namespace Plus.HabboHotel.Users.Ignores;

public sealed class IgnoresComponent
{
    private readonly List<uint> _ignoredUsers;
    public IReadOnlyCollection<uint> IgnoredUsers => _ignoredUsers;

    public event EventHandler<IgnoreStatusUpdatedEventArgs>? UserIgnored;
    public event EventHandler<IgnoreStatusUpdatedEventArgs>? UserUnignored;

    public IgnoresComponent(List<uint> ignoredUsers)
    {
        _ignoredUsers = ignoredUsers;
    }

    public bool TryAdd(uint userId)
    {
        if (_ignoredUsers.Contains(userId))
            return false;
        _ignoredUsers.Add(userId);
        return true;
    }

    public bool IsIgnored(uint userId) => _ignoredUsers.Contains(userId);

    public bool Ignore(uint userId)
    {
        if (_ignoredUsers.Contains(userId)) return false;
        _ignoredUsers.Add(userId);
        UserIgnored?.Invoke(this, new(userId));
        return true;
    }

    public bool Unignore(uint userId)
    {
        if (!_ignoredUsers.Remove(userId)) return false;
        UserUnignored?.Invoke(this, new(userId));
        return true;
    }
}