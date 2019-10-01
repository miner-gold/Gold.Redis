using System;

namespace Gold.Redis.HighLevelClient.Commands.Keys
{
    public class ExpireKeyCommand : BaseKeyCommand
    {
        public TimeSpan Ttl { get; set; }
        public override string GetCommandString() => $"EXPIRE {Key} {Ttl.TotalSeconds}";
    }
}
