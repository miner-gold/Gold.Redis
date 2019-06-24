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
            var seperatedCommands = request.Split(' ');
            var builder = new StringBuilder();

            var requestStartString = $"{RequestCommandPrefixes.Array}{seperatedCommands.Length}{Constants.CrLf}";
            builder.Append(requestStartString);

            foreach (var commandContent in seperatedCommands)
            {
                var command = $"{RequestCommandPrefixes.BulkString}{commandContent.Length}{Constants.CrLf}{commandContent}{Constants.CrLf}";
                builder.Append(command);
            }

            return builder.ToString();
        }
    }
}
