using System.Collections.Generic;

namespace Gold.Redis.LowLevelClient.Interfaces.Parsers
{
    public interface IRequestBuilder
    {
        string Build(params string[] requests);
    }
}
