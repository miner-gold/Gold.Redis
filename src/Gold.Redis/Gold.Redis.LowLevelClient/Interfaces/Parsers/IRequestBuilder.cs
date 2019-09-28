namespace Gold.Redis.LowLevelClient.Interfaces.Parsers
{
    public interface IRequestBuilder
    {
        string Build(string request);
    }
}
