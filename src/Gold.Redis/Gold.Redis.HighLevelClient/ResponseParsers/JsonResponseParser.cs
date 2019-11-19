using System.IO;
using System.Text;
using Gold.Redis.HighLevelClient.Interfaces;
using Newtonsoft.Json;

namespace Gold.Redis.HighLevelClient.ResponseParsers
{
    public class JsonResponseParser : IStringResponseParser
    {
        private readonly JsonSerializer _serializer;
        public JsonResponseParser(JsonSerializerSettings settings = null)
        {
            if (settings != null)
            {
                _serializer = JsonSerializer.Create(settings);
            }
            else
            {
                _serializer = JsonSerializer.CreateDefault();
            }
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

            using (var stringReader = new StringReader(response))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                return _serializer.Deserialize<T>(jsonReader);
            }
        }

        public string Stringify<T>(T item)
        {
            if (item == null)
                return null;

            if (item is string itemStr)
                return itemStr;
            if (item is char itemChar)
                return itemChar.ToString();

            StringBuilder sb = new StringBuilder();
            using (var sr = new StringWriter(sb))
            using (var jsonTextWriter = new JsonTextWriter(sr))
            {
                _serializer.Serialize(jsonTextWriter, item, item.GetType());
                return sb.ToString();
            }
        }
    }
}
