using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common
{
    public static class Constants
    {
        public const string CrLf = "\r\n";
    }


    public enum RedisResponse
    {
        SimpleString = '+',
        Error = '-',
        Integer = ':',
        BulkString = '$',
        Array = '*'
    }
}
