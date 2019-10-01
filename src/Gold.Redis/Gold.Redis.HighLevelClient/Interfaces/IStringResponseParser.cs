using Gold.Redis.LowLevelClient.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IStringResponseParser
    {
        T Parse<T>(string response);
        string Stringify<T>(T item);
    }
}
