using System.Collections.Generic;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetAddCommand : Command
    {
        public string SetKey { get; set; }

        public IEnumerable<string> Items { get; set; }
        public override string GetCommandString() => $"SADD {SetKey} {string.Join(" ", Items)}";
    }
}
