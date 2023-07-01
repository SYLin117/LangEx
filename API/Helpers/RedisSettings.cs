namespace API.Helpers;

public class RedisSettings
{
    public RedisSettings()
    {
    }

    public RedisSettings(string connectionString, string instanceName)
    {
        ConnectionString = connectionString;
        InstanceName = instanceName;
    }

    public string ConnectionString { get; set; }
    public string InstanceName { get; set; }
}