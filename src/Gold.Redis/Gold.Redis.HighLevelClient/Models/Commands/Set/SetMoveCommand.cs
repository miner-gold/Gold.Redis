using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetMoveCommand : Command
    {
        public string SourceSet { get; set; }
        public string DestinationSet { get; set; }
        public string Item { get; set; }

        public override string GetCommandString() => $"SMOVE {SourceSet} {DestinationSet} {Item}";
    }
}
