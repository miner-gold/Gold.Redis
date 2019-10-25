using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Interfaces
{
    public interface IRedisDb : IRedisGeneralOperationsDb,
                                IRedisKeysDb,
                                IRedisSetDb
    {
    }
}
