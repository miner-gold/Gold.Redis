namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetUnionAndStoreCommand : Command
    {
        public string NewSetKey { get; set; }
        public string[] SetKeys { get; set; }
        public override string GetCommandString() => $"SUNIONSTORE {NewSetKey} {string.Join(" ", SetKeys)}";

    }
}
