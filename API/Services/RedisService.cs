using API.Helpers;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Services;

public class RedisService
{
    private readonly ConnectionMultiplexer _redis;

    private readonly IDatabase _database;
    // private readonly JsonSerializerOptions _options;

    public RedisService(IOptions<RedisSettings> config)
    {
        var connectionString = config.Value.ConnectionString;
        var instanceName = config.Value.InstanceName;
        Console.WriteLine($"Redis connStr: {connectionString}");
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _database = _redis.GetDatabase();

        // _options = new JsonSerializerOptions();
        // _options.Converters.Add(new EmptyStringConverter<List<string>>());
    }

    public List<string> GetValue(string key)
    {
        var serializedValues = _database.StringGet(key);
        if (serializedValues.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<List<string>>(serializedValues);
    }

    public void SetValue(string key, string value)
    {
        var serializedValues = JsonSerializer.Deserialize<List<string>>(GetValueOrDefault(key));
        serializedValues.Add(value);
        SetValues(key, serializedValues);
    }

    public void SetValues(string key, List<string> values)
    {
        var serializedValues = JsonSerializer.Serialize(values);

        _database.StringSet(key, serializedValues);
    }

    public bool CheckKey(string key)
    {
        return _database.KeyExists(key);
    }

    public void RemoveValue(string key, string value)
    {
        var serializedValues = GetValueOrDefault(key);
        if (serializedValues.Length <= 2) return;
        var connections = JsonSerializer.Deserialize<List<string>>(serializedValues);
        connections.Remove(value);
        SetValues(key, connections);
        if (connections.Count == 0) _database.KeyDelete(key);
    }

    public int GetConnectionCount(string key)
    {
        var serializedValues = GetValueOrDefault(key);
        if (serializedValues.Length <= 2) return 0;

        return JsonSerializer.Deserialize<List<string>>(serializedValues).Count;
    }

    public List<string> GetAllKeys()
    {
        var server = _redis.GetServer(_redis.GetEndPoints()[0]);
        var keys = server.Keys();

        return keys.Select(key => (string)key).ToList();
    }

    private string GetValueOrDefault(string key)
    {
        var value = _database.StringGet(key);
        return value.HasValue ? value.ToString() : "[]";
    }
}