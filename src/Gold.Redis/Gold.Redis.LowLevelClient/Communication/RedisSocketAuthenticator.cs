using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Gold.Redis.Common;
using Gold.Redis.LowLevelClient.Interfaces;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Communication
{
    public class RedisSocketAuthenticator : IRedisAuthenticator
    {
        private readonly ISocketCommandExecutor _socketCommandExecutor;

        public RedisSocketAuthenticator(ISocketCommandExecutor socketCommandExecutor)
        {
            _socketCommandExecutor = socketCommandExecutor;
        }

        public async Task<bool> TryAuthenticate(Socket connectionSocket, string password)
        {
            var authCommand = "AUTH " + password;
            try
            {
                var response = await _socketCommandExecutor.ExecuteCommand<SimpleStringResponse>(connectionSocket, authCommand);
                return response.Response == Constants.OkResponse;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
