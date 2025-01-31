using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class NumberConfig : Record
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public NumberConfig(int records, string? recordName, int minValue, int maxValue)
        {
            Records = records;
            RecordName = recordName;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
