namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetIntersectAndStoreCommand : Command
    {
        public string NewSetKey { get; set; }
        public string[] SetsKeys { get; set; }
        public override string GetCommandString() => $"SINTERSTORE {NewSetKey} {string.Join(" ", SetsKeys)}";
    }
}
