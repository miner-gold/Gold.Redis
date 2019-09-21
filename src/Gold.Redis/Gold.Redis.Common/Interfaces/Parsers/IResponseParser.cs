using System.IO;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Parsers
{
    public interface IResponseParser
    {
        Task<string> Parse(StreamReader stream);
    }
}
