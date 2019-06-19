using Gold.Redis.Common.Interfaces.Communication;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.Common.Interfaces.Parsers;

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

        public async Task<string> ExecuteCommand(string command)
        {
            using (var socketContainer = await _connections.GetSocket())
            {
                try
                {
                    var bytes = Encoding.ASCII.GetBytes(_requestBuilder.Build(command));
                    var bytesAsArraySegment = new ArraySegment<byte>(bytes);
                    var sendResult = await socketContainer.Socket.SendAsync(bytesAsArraySegment, SocketFlags.None);
                    using (var bufferStream = new BufferedStream(new NetworkStream(socketContainer.Socket), 16 * 1024))
                    {
                        return _responseParser.Parse(bufferStream);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
