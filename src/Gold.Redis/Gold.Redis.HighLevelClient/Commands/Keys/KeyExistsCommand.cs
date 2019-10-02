namespace Gold.Redis.HighLevelClient.Commands.Keys
{
    public class KeyExistsCommand : BaseKeyCommand
    {
        public override string GetCommandString() => $"EXISTS {Key}";
    }
}
