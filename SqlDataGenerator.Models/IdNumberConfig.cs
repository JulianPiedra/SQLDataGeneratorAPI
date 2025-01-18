using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class IdNumberConfig
    {
        public int Records { get; set; }
        public int Lenght { get; set; }
        public bool IsInteger { get; set; }
        public bool HasLetters { get; set; }
        public IdNumberConfig(int records, int lenght, bool isInteger, bool hasLetters)
        {
            Records = records;
            Lenght = lenght;
            IsInteger = isInteger;
            HasLetters = hasLetters;
        }
    }
}
