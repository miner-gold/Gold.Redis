namespace Gold.Redis.Common
{
    public static class Constants
    {
        public const string CrLf = "\r\n";
    }

    public static class CommandPrefixes
    {
        public const char SimpleString = '+';
        public const char Error = '-';
        public const char Integer = ':';
        public const char BulkString = '$';
        public const char Array = '*';
    }
}
