namespace Gold.Redis.HighLevelClient.Models.Commands
{
    public abstract class Command
    {
        public abstract string GetCommandString();
    }
}
