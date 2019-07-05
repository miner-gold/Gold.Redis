using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.Common.Models
{
    public class RedisLowLevelRespons
    {
        public string Message { get; set; }
        public RedisResponse ResponseType{ get; set; }
    }
}
