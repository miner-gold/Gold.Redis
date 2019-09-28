using Gold.Redis.Common.Interfaces.Communication;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Gold.Redis.Common;

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
            try
            {
                var authCommand = "AUTH " + password;
                var response = await _socketCommandExecutor.ExecuteCommand(connectionSocket, authCommand);
                return response == Constants.OkResponse;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
