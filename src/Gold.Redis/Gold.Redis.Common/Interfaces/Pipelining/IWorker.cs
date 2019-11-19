using System.Threading.Tasks;
using Gold.Redis.Common.Utils;

namespace Gold.Redis.Common.Interfaces.Pipelining
{
    public interface IWorker<T,U>
    {
        Task<FutureWork<T, U>> Work(T input);
    }
}
