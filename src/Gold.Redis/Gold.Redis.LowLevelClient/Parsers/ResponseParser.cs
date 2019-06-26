using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.Common.Interfaces.Parsers;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class ResponseParser : IResponseParser
    {
        public async Task<string> Parse(StreamReader stream)
        {
            var firstChar = (char) stream.Read();
            switch (firstChar)
            {
                case CommandPrefixes.SimpleString:
                    return $"{await stream.ReadLineAsync()}";
                case CommandPrefixes.BulkString:
                    var responseLength = await stream.ReadLineAsync();
                    return $"{await stream.ReadLineAsync()}";
                default:
                    return "";
            }
        }
    }
}
