using System.Collections.Generic;

namespace Gold.Redis.LowLevelClient.Responses
{
    public class ArrayResponse : Response
    {
        public IEnumerable<Response> Responses { get; set; }
    }
}