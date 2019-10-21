namespace Gold.Redis.HighLevelClient.Models.Commands.Search
{
    public abstract class ScanCommandBase : Command
    {
        public int Cursor { get; set; } = 0;
        public string Match { get; set; }
        public int? CountHint { get; set; }

        public string GetCountOption() => CountHint == null || CountHint <= 0 ? "" : " COUNT " + CountHint;

        public string GetMatchOption() => string.IsNullOrEmpty(Match) ? "" : " MATCH " + Match;
    }
}
