using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces.Communication
{
    public interface ISocketConnector
    {
        Task ConnectSocket(ISocketContainer socket);
    }
}
