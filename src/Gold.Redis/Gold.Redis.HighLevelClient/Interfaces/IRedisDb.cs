namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisDb : IRedisGeneralOperationsDb,
                                IRedisKeysDb,
                                IRedisSetDb
    {
    }
}
