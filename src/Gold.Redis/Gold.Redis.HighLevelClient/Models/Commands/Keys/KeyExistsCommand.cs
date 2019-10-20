namespace Gold.Redis.HighLevelClient.Models.Commands.Keys
{
    public class KeyExistsCommand : BaseKeyCommand
    {
        public override string GetCommandString() => $"EXISTS {Key}";
    }
}
