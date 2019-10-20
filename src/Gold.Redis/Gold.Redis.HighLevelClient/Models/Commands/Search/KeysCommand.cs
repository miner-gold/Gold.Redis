namespace Gold.Redis.HighLevelClient.Models.Commands.Search
{
    public class KeysCommand : Command
    {
        public string Pattern { get; set; }
        public override string GetCommandString() => $"KEYS {Pattern}";
    }
}
