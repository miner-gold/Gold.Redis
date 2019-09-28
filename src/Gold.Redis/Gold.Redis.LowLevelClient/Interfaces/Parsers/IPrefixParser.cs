using System.IO;
using System.Threading.Tasks;

namespace Gold.Redis.LowLevelClient.Interfaces.Parsers
{
    public interface IPrefixParser
    {
        Task<string> Parse(StreamReader stream);
    }
}
