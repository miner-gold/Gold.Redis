namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetIsMemberExistsCommand : Command
    {
        public string SetKey { get; set; }
        public string Member { get; set; }

        public override string GetCommandString() => $"SISMEMBER {SetKey} {Member}";
    }
}
