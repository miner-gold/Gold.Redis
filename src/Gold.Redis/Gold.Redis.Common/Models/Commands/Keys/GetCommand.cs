namespace Gold.Redis.Common.Models.Commands.Keys 
{
    public class GetCommand : KeysCommand
    {
        public override string GetCommandString() => $"GET {Key}";
    }
}