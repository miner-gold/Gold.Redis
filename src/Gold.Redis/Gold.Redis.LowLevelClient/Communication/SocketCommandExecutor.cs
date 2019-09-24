using Gold.Redis.Common.Interfaces.Communication;
using Gold.Redis.Common.Interfaces.Parsers;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class SocketCommandExecutor : ISocketCommandExecutor
    {
        private readonly IRequestBuilder _requestBuilder;
        private readonly IResponseParser _responseParser;

        public SocketCommandExecutor(
            IRequestBuilder builder,
            IResponseParser parser)
        {
            _requestBuilder = builder;
            _responseParser = parser;
        }
        public async Task<string> ExecuteCommand(Socket socket, string command)
        {
            var bytes = Encoding.ASCII.GetBytes(_requestBuilder.Build(command));
            var bytesAsArraySegment = new ArraySegment<byte>(bytes);
            await socket.SendAsync(bytesAsArraySegment, SocketFlags.None);

            using (var networkStream = new NetworkStream(socket))
            using (var streamReader = new StreamReader(networkStream))
            {
                return await _responseParser.Parse(streamReader);
            }
        }
    }
}
