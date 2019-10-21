using System;

namespace Gold.Redis.HighLevelClient.Models.Commands.Set
{
    public class SetGetRandomMembersCommand : Command
    {
        public string SetKey { get; set; }
        public int NumberOfItems { get; set; }
        public bool AllowMultipleSameItemsReturn { get; set; }
        public override string GetCommandString()
        {
            NumberOfItems = AllowMultipleSameItemsReturn == false ? 
                Math.Abs(NumberOfItems) :
                -Math.Abs(NumberOfItems);

            return $"SRANDMEMBER {SetKey} {NumberOfItems}";
        }
    }
}
