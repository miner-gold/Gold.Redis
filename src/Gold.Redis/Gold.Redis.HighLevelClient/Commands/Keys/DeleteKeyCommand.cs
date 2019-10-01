using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Commands.Keys
{
    public class DeleteKeyCommand : BaseKeyCommand
    {
        public override string GetCommandString() => $"DEL {Key}";
    }
}
