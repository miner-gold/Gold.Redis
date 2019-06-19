using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common
{
    public static class Constants
    {
        public static readonly string CrLf = "\r\n";
    }

    public static class ResponseCommandPrefixes
    {
        public static readonly char SimpleString = '+';
        public static readonly char Error = '-';
        public static readonly char Integer = ':';
    }

    public static class RequestCommandPrefixes
    {
        public static readonly char BulkString = '$';
        public static readonly char Array = '*';
    }
}
