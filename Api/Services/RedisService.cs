using StackExchange.Redis;
namespace Api.Service
{
    public class RedisService(IConnectionMultiplexer redis) : IRedisService
    {
        private readonly IDatabase _db = redis.GetDatabase();


        public async Task SetAsync(string key, string value, TimeSpan expiry)
        {
           
            await _db.StringSetAsync(key, value, expiry);
        }

        public async Task<string?> GetAsync(string key)
        {
            var result = await _db.StringGetAsync(key);
            return result.IsNullOrEmpty ? null : result.ToString();
        }
    }
}
