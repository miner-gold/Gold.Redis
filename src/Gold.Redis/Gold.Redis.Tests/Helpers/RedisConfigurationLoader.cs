using Gold.Redis.Common.Models.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace Gold.Redis.Tests.Helpers
{
    public class RedisConfigurationLoader
    {
        public static RedisConnectionConfiguration GetConfiguration()
        {
            var json = File.ReadAllText("RedisConfig.json");
            return JsonConvert.DeserializeObject<RedisConnectionConfiguration>(json);
        }
    }
}
