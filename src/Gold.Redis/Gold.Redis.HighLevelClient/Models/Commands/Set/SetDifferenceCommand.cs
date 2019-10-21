namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetDifferenceCommand : Command
    {
        public string FirstSetKey { get; set; }
        public string[] DifferentSetKeys { get; set; }


        public override string GetCommandString() => $"SDIFF {FirstSetKey} {string.Join(" ", DifferentSetKeys)}";
    }
}
