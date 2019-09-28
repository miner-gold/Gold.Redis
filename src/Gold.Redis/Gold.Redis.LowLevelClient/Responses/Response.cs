using System.Collections.Generic;

namespace Gold.Redis.LowLevelClient.Responses
{
    public abstract class Response
    {
    }

    public class SimpleStringResponse : Response
    {
        public string Response { get; set; }
    }

    public class ErrorResponse : Response
    {
        public string ErrorMessage { get; set; }
    }

    public class IntResponse : Response
    {
        public int Response { get; set; }
    }

    public class BulkStringResponse : Response //TODO: consider changing to inherit from SimpleStringResponse
    {
        public int StringLength { get; set; }
        public string Response { get; set; }
    }

    public class ArrayResponse : Response
    {
        public IEnumerable<Response> Responses { get; set; }
    }
}
