using Gold.Redis.Common.Interfaces.Communication;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class RedisLowLevelClient : IRedisConnection
    {
        private readonly IConnectionsContainer _connections;

        public RedisLowLevelClient(IConnectionsContainer connections)
        {
            _connections = connections;
        }

        public async Task<string> ExecuteCommand(string command)
        {
            using (var socketContainer = await _connections.GetSocket())
            {
                try
                {
                    var bytes = Encoding.ASCII.GetBytes(command);
                    var bytesAsArraySegment = new ArraySegment<byte>(bytes);
                    var sendResult = await socketContainer.Socket.SendAsync(bytesAsArraySegment, SocketFlags.None);
                    using (var bufferStream = new BufferedStream(new NetworkStream(socketContainer.Socket), 16 * 1024))
                    {
                        return ReadResponseFromStream(bufferStream);
                    }
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }

        private string ReadResponseFromStream(BufferedStream bufferStream)
        {
            var sb = new StringBuilder();

            int current;
            var prev = default(char);
            while ((current = bufferStream.ReadByte()) != -1)
            {
                var c = (char)current;
                if (prev == '\r' && c == '\n') // reach at TerminateLine
                {
                    break;
                }
                else if (prev == '\r' && c == '\r')
                {
                    sb.Append(prev); // append prev '\r'
                    continue;
                }
                else if (c == '\r')
                {
                    prev = c; // not append '\r'
                    continue;
                }

                prev = c;
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
