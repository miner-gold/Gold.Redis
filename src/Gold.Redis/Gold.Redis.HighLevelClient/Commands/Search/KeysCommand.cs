using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Commands.Search
{
    public class KeysCommand : Command
    {
        public string Pattern { get; set; }
        public override string GetCommandString() => $"KEYS {Pattern}";
    }
}
