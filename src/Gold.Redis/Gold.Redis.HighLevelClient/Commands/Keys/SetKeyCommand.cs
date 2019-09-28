namespace Gold.Redis.HighLevelClient.Commands.Keys
{
    public class SetKeyCommand : KeysCommand
    {
        public string Value { get; set; }
        public override string GetCommandString() => $"SET {Key} \"{Value}\"";
    }
}
