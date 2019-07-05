using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Gold.Redis.Common;
using Gold.Redis.Common.Models;

namespace Gold.Redis.Tests.AssertExtensions
{
    public class RedisLowLevelResponseAssertions : ReferenceTypeAssertions<RedisLowLevelRespons, RedisLowLevelResponseAssertions>
    {
        protected override string Identifier => "response";

        public RedisLowLevelResponseAssertions(RedisLowLevelRespons subject) : base(subject) { }

        public AndConstraint<RedisLowLevelResponseAssertions> MessageBe(string message, RedisResponse responseType)
        {
            Execute.Assertion
                .ForCondition(Subject.Message.Equals(message))
                .FailWith($"Expected message to be {message} but found {Subject.Message}")
                .Then
                .ForCondition(Subject.ResponseType.Equals(responseType))
                .FailWith($"Expected response type to be {responseType} but found {Subject.ResponseType}");

            return new AndConstraint<RedisLowLevelResponseAssertions>(this);
        }
    }

    public static class RedisLowLevelResponseExtensions
    {
        public static RedisLowLevelResponseAssertions Should(this RedisLowLevelRespons response)
        {
            return new RedisLowLevelResponseAssertions(response);
        }
    }
}
