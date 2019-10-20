namespace Gold.Redis.HighLevelClient.Models.Commands.Keys 
{
    public class GetKeyValueCommand : BaseKeyCommand
    {
        public override string GetCommandString() => $"GET {Key}";
    }
}