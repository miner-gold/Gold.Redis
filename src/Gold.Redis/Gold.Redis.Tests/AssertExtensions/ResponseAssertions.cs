using System;
using FluentAssertions;
using FluentAssertions.Primitives;
using Gold.Redis.LowLevelClient.Responses;

namespace Gold.Redis.Tests.AssertExtensions
{
    public class ResponseAssertions : ReferenceTypeAssertions<Response, ResponseAssertions>
    {
        protected override string Identifier => "Response";

        public ResponseAssertions(Response subject) : base(subject) { }

        public AndConstraint<ResponseAssertions> Be(string message, Response responseType)
        {
            throw new NotImplementedException();
            return new AndConstraint<ResponseAssertions>(this);
        }
    }

    public static class RedisLowLevelResponseExtensions
    {
        public static ResponseAssertions Should(this Response response)
        {
            return new ResponseAssertions(response);
        }
    }
}
