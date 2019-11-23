namespace Gold.Redis.LowLevelClient.Interfaces.Parsers
{
    public interface IRequestBuilder
    {
        string Build(params string[] requests);
    }
}
