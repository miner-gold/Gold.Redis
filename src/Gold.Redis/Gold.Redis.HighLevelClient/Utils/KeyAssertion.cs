using System;
using System.Collections.Generic;
using System.Text;

namespace Gold.Redis.HighLevelClient.Utils
{
    public enum KeyAssertion
    {
        Any,
        MustExist,
        MustNotExists
    }
}
