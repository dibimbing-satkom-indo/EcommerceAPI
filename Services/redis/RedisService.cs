using StackExchange.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task SetStringAsync(string key, string value)
    {
        await _db.StringSetAsync(key, value, TimeSpan.FromMinutes(10)); // expired 10
    }

    public async Task<string?> getStringAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }
}