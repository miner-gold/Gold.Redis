using System.IO;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Parsers.PrefixParsers
{
    public class SimpleStringParser : IPrefixParser
    {
        public async Task<Response> Parse(StreamReader stream)
        {
            return new SimpleStringResponse
            {
                Response = await stream.ReadLineAsync()
            };
        }
    }
}