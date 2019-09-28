using Gold.Redis.Common.Models.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace Gold.Redis.Tests.Helpers
{
    public class RedisConfigurationLoader
    {
        private const string FILE_PATH = "RedisConfig.json";
        public static RedisConnectionConfiguration GetConfiguration()
        {
            if (!File.Exists(FILE_PATH))
                return new RedisConnectionConfiguration();

            var json = File.ReadAllText(FILE_PATH);
            return JsonConvert.DeserializeObject<RedisConnectionConfiguration>(json);
        }
    }
}
