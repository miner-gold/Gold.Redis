namespace Gold.Redis.Common.Models.Commands.Keys
{
    public abstract class KeysCommand : Command
    {
        public KeysCommand()
        {
            Type = CommandType.Keys;
        }

        public string Key { get; set; }
    }
}
