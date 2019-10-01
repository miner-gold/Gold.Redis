namespace Gold.Redis.HighLevelClient.Commands.Keys 
{
    public class GetKeyValueCommand : BaseKeyCommand
    {
        public override string GetCommandString() => $"GET {Key}";
    }
}