namespace Gold.Redis.Common.Models.Commands
{
    public abstract class Command
    {
        public CommandType Type { get; protected set; }
        public abstract string GetCommandString();
    }
}
