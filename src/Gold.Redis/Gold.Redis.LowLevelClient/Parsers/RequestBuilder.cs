using System;
using System.Collections.Generic;
using System.Text;
using Gold.Redis.Common;
using Gold.Redis.Common.Interfaces.Parsers;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class RequestBuilder : IRequestBuilder
    {

        public string Build(string request)
        {
            var splicedCommand = request.Split(' ');
            var builder = new StringBuilder();
            builder.Append($"{RequestCommandPrefixes.Array}{splicedCommand.Length}{Constants.CrLf}");
            foreach (var splice in splicedCommand)
            {
                builder.Append(
                    $"{RequestCommandPrefixes.BulkString}{splice.Length}{Constants.CrLf}{splice}{Constants.CrLf}");
            }

            return builder.ToString();
        }
    }
}
