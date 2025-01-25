using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class NumberConfig : Record
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public NumberConfig(int records, int minValue, int maxValue)
        {
            Records = records;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
