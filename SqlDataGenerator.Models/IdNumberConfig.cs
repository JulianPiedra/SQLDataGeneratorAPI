using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class IdNumberConfig : Record
    {
        public int Length { get; set; }
        public bool HasLetters { get; set; }
        public IdNumberConfig(int records, int length, bool hasLetters)
        {
            Records = records;
            Length = length;
            HasLetters = hasLetters;
        }
    }
}
