using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gold.Redis.Common;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;

namespace Gold.Redis.LowLevelClient.Parsers
{
    public class RequestBuilder : IRequestBuilder
    {
        private string BuildSingleRequest(string request)
        {
            var separatedCommands = request.Split(' ');
            var builder = new StringBuilder();

            var requestStartString = $"{CommandPrefixes.Array}{separatedCommands.Length}{Constants.CrLf}";
            builder.Append(requestStartString);

            foreach (var commandContent in separatedCommands)
            {
                var command = $"{CommandPrefixes.BulkString}{commandContent.Length}{Constants.CrLf}{commandContent}{Constants.CrLf}";
                builder.Append(command);
            }

            return builder.ToString();
        }

        public string Build(params string[] requests)
        {
            var modifiedRequests = requests.Select(BuildSingleRequest).ToList();
            if (modifiedRequests.Count == 1)
            {
                return modifiedRequests[0];
            }

            var stringBuilder = new StringBuilder(modifiedRequests.Count);
            foreach (var command in modifiedRequests)
            {
                stringBuilder.AppendLine(command);
            }

            return stringBuilder.ToString();
        }
    }
}
