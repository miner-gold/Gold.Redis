using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Models.Commands.General
{
    public class PingCommand : Command
    {
        public override string GetCommandString() => "PING";
    }
}
