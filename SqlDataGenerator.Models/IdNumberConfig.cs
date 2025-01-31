using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class IdNumberConfig : Record
    {
        public int Length { get; set; }
        public bool HasLetters { get; set; }
        public IdNumberConfig(int records, string? recordName, int length, bool hasLetters)
        {
            Records = records;
            RecordName = recordName;
            Length = length;
            HasLetters = hasLetters;
        }
    }
}
