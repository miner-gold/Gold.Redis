using Gold.Redis.Common.Interfaces.Communication;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common.Interfaces.Parsers;
using Gold.Redis.Common.Models;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class RedisLowLevelClient : IRedisConnection
    {
        private readonly IConnectionsContainer _connections;
        private readonly IRequestBuilder _requestBuilder;
        private readonly IResponseParser _responseParser;

        public RedisLowLevelClient(IConnectionsContainer connections, IRequestBuilder requestBuilder,
            IResponseParser responseParser)
        {
            _connections = connections;
            _requestBuilder = requestBuilder;
            _responseParser = responseParser;
        }

        public async Task<RedisLowLevelResponse> ExecuteCommand(string command)
        {
            using (var socketContainer = await _connections.GetSocket())
            {
                var bytes = Encoding.ASCII.GetBytes(_requestBuilder.Build(command));
                var bytesAsArraySegment = new ArraySegment<byte>(bytes);
                var sendResult = await socketContainer.Socket.SendAsync(bytesAsArraySegment, SocketFlags.None);

                using (var networkStream = new NetworkStream(socketContainer.Socket))
                using (var streamReader = new StreamReader(networkStream))
                {
                    return await _responseParser.Parse(streamReader);
                }
            }
        }
    }
}
