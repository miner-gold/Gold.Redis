using System;

namespace Gold.Redis.HighLevelClient.Models.Commands.Keys
{
    public class ExpireKeyCommand : BaseKeyCommand
    {
        public TimeSpan Ttl { get; set; }
        public override string GetCommandString() => $"EXPIRE {Key} {Ttl.TotalSeconds}";
    }
}
