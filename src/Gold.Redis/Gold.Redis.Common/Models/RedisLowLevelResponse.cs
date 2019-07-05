using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common.Models
{
    public class RedisLowLevelResponse
    {
        public string Message { get; set; }
        public RedisResponseTypes ResponseType{ get; set; }
    }
}
