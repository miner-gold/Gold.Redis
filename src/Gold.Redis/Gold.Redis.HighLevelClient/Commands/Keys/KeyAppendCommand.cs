namespace Gold.Redis.HighLevelClient.Commands.Keys
{
    public class KeyAppendCommand : BaseKeyCommand
    {
        public string Value { get; set; }
        public override string GetCommandString() => $"APPEND {Key} \"{Value}\"";

    }
}
