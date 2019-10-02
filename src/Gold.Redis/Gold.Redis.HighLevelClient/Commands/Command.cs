namespace Gold.Redis.HighLevelClient.Commands
{
    public abstract class Command
    {
        public abstract string GetCommandString();
    }
}
