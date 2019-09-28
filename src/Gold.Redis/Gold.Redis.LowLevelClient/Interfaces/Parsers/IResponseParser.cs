using System.IO;
using System.Threading.Tasks;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.LowLevelClient.Interfaces.Parsers
{
    public interface IResponseParser
    {
        Task<Response> Parse(StreamReader stream);
    }
}
