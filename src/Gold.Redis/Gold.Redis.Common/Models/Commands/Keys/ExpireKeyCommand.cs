using System;

namespace Gold.Redis.Common.Models.Commands.Keys
{
    public class ExpireKeyCommand : KeysCommand
    {
        public TimeSpan Ttl { get; set; }
        public override string GetCommandString() => $"EXPIRE {Key} {Ttl.TotalSeconds}";
    }
}
