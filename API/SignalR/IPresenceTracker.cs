namespace API.SignalR;

public interface IPresenceTracker
{
    public Task<bool> UserConnected(string username, string connectionId);
    public Task<bool> UserDisconnected(string username, string connectionId);
    public Task<string[]> GetOnlineUsers();
    public Task<List<string>> GetConnectionsForUser(string username);
}