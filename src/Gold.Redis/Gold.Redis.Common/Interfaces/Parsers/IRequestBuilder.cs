using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common.Interfaces.Parsers
{
    public interface IRequestBuilder
    {
        string Build(string request);
    }
}
