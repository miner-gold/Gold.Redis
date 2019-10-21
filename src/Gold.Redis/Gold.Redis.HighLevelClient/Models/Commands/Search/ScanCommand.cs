namespace Gold.Redis.HighLevelClient.Models.Commands.Search
{
    public class ScanCommand : ScanCommandBase
    {
        public string Type { get; set; }

        public override string GetCommandString() => "SCAN " + Cursor + "" + GetMatchOption() + "" + GetCountOption() + "" + GetTypeOption();

        private string GetTypeOption() => string.IsNullOrEmpty(Type) ? "" : " TYPE " + Type;
    }
}
