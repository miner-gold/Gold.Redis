using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Interfaces.Parsers;
using Gold.Redis.LowLevelClient.Responses;

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
        public async Task<T> ExecuteCommand<T>(Socket socket, string command)
            where T : Response
        {
            var bytes = Encoding.ASCII.GetBytes(_requestBuilder.Build(command));
            var bytesAsArraySegment = new ArraySegment<byte>(bytes);
            await socket.SendAsync(bytesAsArraySegment, SocketFlags.None);

            using (var networkStream = new NetworkStream(socket))
            using (var streamReader = new StreamReader(networkStream))
            {
                var response = await _responseParser.Parse(streamReader);

                if (response == null)
                    return null;

                if (response is T typedResponse)
                {
                    return typedResponse;
                }

                if (response is ErrorResponse errorResponse)
                {
                    throw new Exception(errorResponse.ErrorMessage);
                }

                throw new InvalidCastException($"Could not cast response to the desired type." +
                                               $" Expected type: ${typeof(T)} Actual type: ${response?.GetType()}");
            }
        }
    }
}
