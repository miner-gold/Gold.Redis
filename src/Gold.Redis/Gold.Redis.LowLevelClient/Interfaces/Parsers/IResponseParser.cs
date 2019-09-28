using System.IO;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces.Parsers
{
    public interface IResponseParser
    {
        Task<string> Parse(StreamReader stream);
    }
}
