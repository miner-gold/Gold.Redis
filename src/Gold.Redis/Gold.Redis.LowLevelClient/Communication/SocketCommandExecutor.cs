using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<T>> ExecuteCommands<T>(Socket socket, params string[] commands)
            where T : Response
        {
            var responses = new T[commands.Length];
            var arraySegment = GetCommandArraySegment<T>(commands);
            await socket.SendAsync(arraySegment, SocketFlags.None);

            using (var networkStream = new NetworkStream(socket))
            using (var streamReader = new StreamReader(networkStream))
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    var response = await _responseParser.Parse(streamReader);

                    if (response == null)
                        return null;

                    if (response is ErrorResponse errorResponse)
                    {
                        throw new Exception(errorResponse.ErrorMessage);
                    }

                    responses[i] = (T)response;
                }
            }

            return responses;
        }

        public async Task<T> ExecuteCommand<T>(Socket socket, string command) where T : Response
        {
            var arraySegment = GetCommandArraySegment<T>(command);
            await socket.SendAsync(arraySegment, SocketFlags.None);

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

        private ArraySegment<byte> GetCommandArraySegment<T>(params string[] commands) where T : Response
        {
            var bytes = Encoding.ASCII.GetBytes(_requestBuilder.Build(commands));
            return new ArraySegment<byte>(bytes);
        }
    }
}
