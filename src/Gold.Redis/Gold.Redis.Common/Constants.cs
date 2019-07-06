using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common
{
    public static class Constants
    {
        public const string CrLf = "\r\n";
        public const string MovedError = "MOVED";
    }

    public enum RedisResponseTypes
    {
        SimpleString = '+',
        Error = '-',
        Integer = ':',
        BulkString = '$',
        Array = '*'
    }

    public enum MessageType
    {
        Moved,
        Ask,
    }
}
