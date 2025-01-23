using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class IdNumberConfig : Record
    {
        public int Lenght { get; set; }
        public bool HasLetters { get; set; }
        public IdNumberConfig(int records, int lenght, bool hasLetters)
        {
            Records = records;
            Lenght = lenght;
            HasLetters = hasLetters;
        }
    }
}
