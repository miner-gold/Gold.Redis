using System.IO;
using System.Text;
using System.Text.Json;
using Gold.Redis.HighLevelClient.Interfaces;


namespace Gold.Redis.HighLevelClient.ResponseParsers
{
    public class JsonResponseParser : IStringResponseParser
    {
        private readonly JsonSerializerOptions _options;
        public JsonResponseParser(JsonSerializerOptions options = null)
        {
            _options = options;
        }
        public T Parse<T>(string response)
        {
            if (response == null)
                return default(T);

            if (typeof(T) == typeof(string))
            {
                //In order to simply deserialize string to string
                response = "\"" + response + "\"";
            }

            return JsonSerializer.Deserialize<T>(response, _options);
        }

        public string Stringify<T>(T item)
        {
            if (item == null)
                return null;

            if (item is string itemStr)
                return itemStr;
            if (item is char itemChar)
                return itemChar.ToString();

            return JsonSerializer.Serialize<T>(item, _options);
        }
    }
}
