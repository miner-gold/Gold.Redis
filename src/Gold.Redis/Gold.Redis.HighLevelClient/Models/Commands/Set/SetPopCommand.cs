namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetPopCommand : Command
    {
        public string SetKey { get; set; }
        public int NumberOfElements { get; set; }
        public override string GetCommandString() => $"SPOP {SetKey} {NumberOfElements}";
    }
}
