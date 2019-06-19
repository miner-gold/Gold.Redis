using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gold.Redis.Common.Interfaces.Parsers
{
    public interface IResponseParser
    {
        string Parse(Stream stream);
    }
}
