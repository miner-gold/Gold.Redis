namespace Gold.Redis.Common.Models.Commands.Keys
{
    public class SetKeyCommand : KeysCommand
    {
        public string Value { get; set; }
        public override string GetCommandString() => $"SET {Key} \"{Value}\"";
    }
}
