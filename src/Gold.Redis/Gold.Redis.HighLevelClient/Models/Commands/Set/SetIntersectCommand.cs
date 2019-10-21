namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetIntersectCommand : Command
    {
        public string[] SetsKeys { get; set; }
        public override string GetCommandString() => $"SINTER {string.Join(" ", SetsKeys)}";
    }
}
