using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetRemoveCommand : Command
    {
        public string SetKey { get; set; }

        public string[] ItemsToRemove { get; set; }
        public override string GetCommandString() => $"SREM {SetKey} {string.Join(" ", ItemsToRemove)}";
    }
}
