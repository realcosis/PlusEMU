using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plus.HabboHotel.GameClients;
using System.Threading.Tasks;
using Dapper;
using Plus.Communication.Packets.Outgoing.BuildersClub;
using Plus.Communication.Packets.Outgoing.Handshake;
using Plus.Communication.Packets.Outgoing.Inventory.Achievements;
using Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.Communication.Packets.Outgoing.Notifications;
using Plus.Communication.Packets.Outgoing.Sound;
using Plus.Core;
using Plus.Database;
using Plus.HabboHotel.Users.Messenger.FriendBar;
using Plus.HabboHotel.Users.UserData;

namespace Plus.HabboHotel.Users.Authentication
{
    internal class Authenticator : IAuthenticator
    {
        private readonly IEnumerable<IAuthenticationTask> _authenticationTasks;
        private readonly IGameClientManager _gameClientManager;
        private readonly IUserDataFactory _userDataFactory;
        private readonly IDatabase _database;

        public event EventHandler<HabboEventArgs>? HabboLoggedIn;

        public Authenticator(IEnumerable<IAuthenticationTask> authenticationTasks, IGameClientManager gameClientManager, IUserDataFactory userDataFactory, IDatabase database)
        {
            _authenticationTasks = authenticationTasks;
            _gameClientManager = gameClientManager;
            _userDataFactory = userDataFactory;
            _database = database;
        }

        public async Task<AuthenticationError?> AuthenticateUsingSSO(GameClient session, string sso)
        {
            sso = sso.Trim();
            if (string.IsNullOrEmpty(sso))
                return AuthenticationError.EmptySSO;//|| sso.Length < 15)

            if (!Debugger.IsAttached && sso.Length < 15)
                return AuthenticationError.InvalidSSO;

            var userId = await GetUserIdFromSso(sso);
            if (userId == default)
                return AuthenticationError.NoAccountFound;

            if (!Debugger.IsAttached)
                await ResetSso(userId);

            var canLogin = await CanLogin(userId);
            if (!canLogin)
                return AuthenticationError.LoginProhibited;

            var habbo = await _userDataFactory.Create(userId);
            if (habbo == null)
                return AuthenticationError.NoAccountFound;

            session.SetHabbo(habbo);

            // TODO: Remove after splitting up
            habbo.Init(session);
            _gameClientManager.RegisterClient(session, habbo.Id, habbo.Username);
            await RaiseHabboLoggedIn(habbo);
            return null;
        }

        private async Task<int> GetUserIdFromSso(string sso)
        {
            using var connection = _database.Connection();
            return await connection.ExecuteScalarAsync<int>("SELECT id FROM users WHERE auth_ticket = @sso", new { sso });
        }

        private async Task ResetSso(int userId)
        {
            using var connection = _database.Connection();
            await connection.ExecuteAsync("UPDATE users SET auth_ticket = NULL WHERE id = @userId", new { userId });
        }

        private async Task<bool> CanLogin(int userId)
        {
            foreach (var task in _authenticationTasks)
            {
                if (!(await task.CanLogin(userId)))
                    return false;
            }
            return true;
        }

        private async Task RaiseHabboLoggedIn(Habbo habbo)
        {
            foreach (var task in _authenticationTasks)
                await task.UserLoggedIn(habbo);
            HabboLoggedIn?.Invoke(this, new(habbo));
        }
    }
}
