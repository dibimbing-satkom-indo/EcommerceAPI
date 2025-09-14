using StackExchange.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    private ILogger<RedisService> _log;
    public RedisService(IConnectionMultiplexer redis, ILogger<RedisService> log)
    {
        _db = redis.GetDatabase();
        _log = log;
    }

    public async Task SetStringAsync(string key, string value)
    {
        _log.LogWarning($"key :{key}, value : {value}");

        await _db.StringSetAsync(key, value, TimeSpan.FromMinutes(10)); // expired 10
    }

    public async Task<string?> getStringAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }
}