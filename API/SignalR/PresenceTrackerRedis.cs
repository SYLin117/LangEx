using API.Services;

namespace API.SignalR
{
    /// <summary>
    /// use to track people online
    /// </summary>
    public class PresenceTrackerRedis : IPresenceTracker
    {
        private RedisService _redisService;

        public PresenceTrackerRedis(RedisService redisService)
        {
            _redisService = redisService;
        }

        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool isOnline = false;

            if (_redisService.CheckKey(username))
            {
                _redisService.SetValue(username, connectionId);
            }
            else
            {
                _redisService.SetValue(username,connectionId);
                isOnline = true;
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;

            if (!_redisService.CheckKey(username)) return Task.FromResult(isOffline);
            _redisService.RemoveValue(username, connectionId);
            if (_redisService.GetConnectionCount(username) == 0)
            {
                isOffline = true;
            }

            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;

            onlineUsers = _redisService.GetAllKeys().ToArray();

            return Task.FromResult(onlineUsers);
        }

        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;

            connectionIds = _redisService.GetValue(username);

            return Task.FromResult(connectionIds);
        }
    }
}