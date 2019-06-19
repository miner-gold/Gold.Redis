using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gold.Redis.Common.Interfaces.Parsers;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class ResponseParser : IResponseParser
    {
        public string Parse(Stream stream)
        {
            var stringBuilder = new StringBuilder();
            int current;
            var prev = default(char);
            while ((current = stream.ReadByte()) != -1)
            {
                var c = (char)current;
                if (prev == '\r' && c == '\n') // reach at TerminateLine
                {
                    break;
                }

                if (prev == '\r' && c == '\r')
                {
                    stringBuilder.Append(prev); // append prev '\r'
                    continue;
                }

                if (c == '\r')
                {
                    prev = c; // not append '\r'
                    continue;
                }

                prev = c;
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }
    }
}
