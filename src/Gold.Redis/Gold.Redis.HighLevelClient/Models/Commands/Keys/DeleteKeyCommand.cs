namespace Gold.Redis.HighLevelClient.Models.Commands.Keys
{
    public class DeleteKeyCommand : BaseKeyCommand
    {
        public override string GetCommandString() => $"DEL {Key}";
    }
}
