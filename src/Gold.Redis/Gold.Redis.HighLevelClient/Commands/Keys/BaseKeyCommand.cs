namespace Gold.Redis.HighLevelClient.Commands.Keys
{
    public abstract class BaseKeyCommand : Command
    {
        public string Key { get; set; }
    }
}
