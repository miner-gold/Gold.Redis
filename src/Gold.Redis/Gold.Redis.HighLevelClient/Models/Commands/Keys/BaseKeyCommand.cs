namespace Gold.Redis.HighLevelClient.Models.Commands.Keys
{
    public abstract class BaseKeyCommand : Command
    {
        public string Key { get; set; }
    }
}
