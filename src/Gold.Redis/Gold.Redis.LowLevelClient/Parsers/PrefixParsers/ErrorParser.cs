using System.IO;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;

namespace Gold.Redis.LowLevelClient.Parsers.PrefixParsers
{
    public class ErrorParser : IPrefixParser
    {
        public async Task<string> Parse(StreamReader stream)
        {
            return await stream.ReadLineAsync();
        }
    }
}