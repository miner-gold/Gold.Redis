using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetIntersectAndStoreCommand : Command
    {
        public string NewSetKey { get; set; }
        public string[] SetsKeys { get; set; }
        public override string GetCommandString() => $"SINTER {NewSetKey} {string.Join(" ", SetsKeys)}";
    }
}
