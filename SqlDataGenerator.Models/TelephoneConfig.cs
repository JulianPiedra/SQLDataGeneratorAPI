using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class TelephoneConfig : Record
    {
        public int Length { get; set; }
        public bool IncludeCode { get; set; }
        public TelephoneConfig(int records,string? recordName, int length, bool includeCode)
        {
            Records = records;
            RecordName = recordName;
            Length = length;
            IncludeCode = includeCode;
        }
    }
}
