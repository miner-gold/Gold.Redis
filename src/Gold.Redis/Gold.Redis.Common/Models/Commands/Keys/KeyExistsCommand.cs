namespace Gold.Redis.Common.Models.Commands.Keys
{
    public class KeyExistsCommand : KeysCommand
    {
        public override string GetCommandString() => $"EXISTS {Key}";
    }
}
