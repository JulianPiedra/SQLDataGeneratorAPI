using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class TelephoneConfig : Record
    {
        public int Length { get; set; }
        public bool IncludeCode { get; set; }
        public TelephoneConfig(int records, int length, bool includeCode)
        {
            Records = records;
            Length = length;
            IncludeCode = includeCode;
        }
    }
}
