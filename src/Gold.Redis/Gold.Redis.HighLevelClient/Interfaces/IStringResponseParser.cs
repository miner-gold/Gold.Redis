namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IStringResponseParser
    {
        T Parse<T>(string response);
        string Stringify<T>(T item);
    }
}
