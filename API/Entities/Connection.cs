namespace API.Entities
{
    public class Connection
    {
        // entity framework need a empty constructor
        public Connection()
        {
            
        }
        public Connection(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }

        public string ConnectionId { get; set; } // signalR connectionID
        public string Username { get; set; } // username for this connection
    }
}