namespace Gold.Redis.HighLevelClient.Commands.Keys 
{
    public class GetCommand : KeysCommand
    {
        public override string GetCommandString() => $"GET {Key}";
    }
}