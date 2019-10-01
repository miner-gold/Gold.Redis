namespace Gold.Redis.HighLevelClient.Commands.General
{
    public class FlushDbCommand : Command
    {
        public bool IsAsync { get; set; }
        public override string GetCommandString() =>
            IsAsync ? "FLUSHDB ASYNC" : "FLUSHDB";
    }
}
