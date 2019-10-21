using System;
using Gold.Redis.HighLevelClient.Models.Utils;

namespace Gold.Redis.HighLevelClient.Models.Commands.Keys
{
    public class SetKeyCommand : BaseKeyCommand
    {

        public KeyAssertion Assertion { get; set; }
        public TimeSpan? ExpirySpan { get; set; }
        public string Value { get; set; }
        public override string GetCommandString() => $"SET {Key} \"{Value}\"{GetExpiratoryMillisecondsString()}{GetKeyAssertionString()}";

        private string GetKeyAssertionString()
        {
            switch (Assertion)
            {
                case KeyAssertion.Any: return "";
                case KeyAssertion.MustExist: return " XX";
                case KeyAssertion.MustNotExists: return " NX";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetExpiratoryMillisecondsString()
        {
            return ExpirySpan.HasValue ?
                $" PX {ExpirySpan.Value.TotalMilliseconds}" :
                "";
        }
    }
}
