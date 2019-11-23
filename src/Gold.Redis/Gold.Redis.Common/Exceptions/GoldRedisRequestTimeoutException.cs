using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common.Exceptions
{
    public class GoldRedisRequestTimeoutException : Exception
    {
        public GoldRedisRequestTimeoutException(string hostName, TimeSpan requestTimeout, params string[] commands) : base(
            
            $"Exceeded maximum timeout while trying to preform {GetCommandOrCommands(commands.Length)}:" +
            $" {string.Join(",", commands)}. \nMaximum request timeout: {requestTimeout}, redis host: {hostName}")
        {

        }

        private static string GetCommandOrCommands(int length) => length > 1 ? "commands" : "command";
    }
}
