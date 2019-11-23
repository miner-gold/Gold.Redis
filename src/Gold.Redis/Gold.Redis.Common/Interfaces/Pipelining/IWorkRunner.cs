using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Pipelining
{
    public interface IWorkRunner<T,U>
    {
        Task<U> ExecuteWork(T input);
    }
}
