namespace Gold.Redis.LowLevelClient.Responses
{
    public class BulkStringResponse : Response
    {
        public int StringLength { get; set; }
        public string Response { get; set; }
    }
}