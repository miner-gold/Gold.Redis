namespace Gold.Redis.HighLevelClient.Commands.Keys
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
