using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetAddCommand : Command
    {
        public string SetKey { get; set; }

        public IEnumerable<string> Items { get; set; }
        public override string GetCommandString() => $"SET {SetKey} {string.Join(" ", Items)}";
    }
}
