using System.Threading.Tasks;

namespace Gold.Redis.Common.Utils
{
    public class FutureWork<T, U>
    {
        public T Item { get; set; }
        public TaskCompletionSource<U> Work { get; set; }
        public Task<U> WorkDone => Work.Task;

        public FutureWork(T input)
        {
            Item = input;
            Work = new TaskCompletionSource<U>();
        }
    }
}
