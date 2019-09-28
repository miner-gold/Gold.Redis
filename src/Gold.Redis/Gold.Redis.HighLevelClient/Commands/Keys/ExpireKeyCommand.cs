using System;

namespace Gold.Redis.HighLevelClient.Commands.Keys
{
    public class ExpireKeyCommand : KeysCommand
    {
        public TimeSpan Ttl { get; set; }
        public override string GetCommandString() => $"EXPIRE {Key} {Ttl.TotalSeconds}";
    }
}
