using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetDifferenceAndStoreCommand : Command
    {
        public string NewSetKey { get; set; }
        public string FirstSetKey { get; set; }
        public string[] DifferentSetKeys { get; set; }


        public override string GetCommandString() => $"SDIFFSTORE {NewSetKey} {FirstSetKey} {string.Join(" ", DifferentSetKeys)}";
    }
}
