using System.IO;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;

namespace Gold.Redis.LowLevelClient.Parsers.PrefixParsers
{
    public class BulkStringParser : IPrefixParser
    {
        public async Task<string> Parse(StreamReader stream)
        {
            var responseLength = await stream.ReadLineAsync();
            return await stream.ReadLineAsync();
        }
    }
}