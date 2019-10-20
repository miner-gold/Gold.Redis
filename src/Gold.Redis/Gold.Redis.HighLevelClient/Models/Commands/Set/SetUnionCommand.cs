using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetUnionCommand : Command
    {
        public string[] SetKeys { get; set; }
        public override string GetCommandString() => $"SUNION {string.Join(" ", SetKeys)}";
    }
}
