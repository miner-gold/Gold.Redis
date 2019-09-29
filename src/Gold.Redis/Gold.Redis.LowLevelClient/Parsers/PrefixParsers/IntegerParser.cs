using System;
using System.IO;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Parsers.PrefixParsers
{
    public class IntegerParser : IPrefixParser
    {
        public async Task<Response> Parse(StreamReader stream)
        {
            var response = await stream.ReadLineAsync();
            if (int.TryParse(response, out var result))
            {
                return new IntResponse
                {
                    Response = result
                };
            }

            throw new Exception("Expected int response but found: " + response);
        }
    }
}