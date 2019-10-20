using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetCardinalityCommand : Command
    {
        public string SetKey { get; set; }
        public override string GetCommandString() => $"SCARD {SetKey}";
    }
}
