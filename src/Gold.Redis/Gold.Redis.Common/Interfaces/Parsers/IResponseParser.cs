using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gold.Redis.Common.Interfaces.Parsers
{
    public interface IResponseParser
    {
        Task<string> Parse(StreamReader stream);
    }
}
