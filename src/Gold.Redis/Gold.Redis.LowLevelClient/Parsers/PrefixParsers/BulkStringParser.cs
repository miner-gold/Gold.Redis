using System;
using System.IO;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Parsers.PrefixParsers
{
    public class BulkStringParser : IPrefixParser
    {
        public async Task<Response> Parse(StreamReader stream)
        {
            var responseLength = await stream.ReadLineAsync();
            if (int.TryParse(responseLength, out var length))
            {
                if (length == -1)
                    return null;

                return new BulkStringResponse
                {
                    StringLength = length,
                    Response = await stream.ReadLineAsync()
                };
            }

            throw new Exception("Expected string length to be int but found: " + responseLength);
        }
    }
}