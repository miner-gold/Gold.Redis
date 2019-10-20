namespace Gold.Redis.HighLevelClient.Models.Commands.General
{
    public class FlushDbCommand : Command
    {
        public bool IsAsync { get; set; }
        public override string GetCommandString() =>
            IsAsync ? "FLUSHDB ASYNC" : "FLUSHDB";
    }
}
