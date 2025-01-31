using System.Numerics;

namespace SqlDataGenerator.Models
{
    public class DateConfig : Record
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public bool IncludeTime { get; set; }
        public DateConfig(int records, string? recordName, DateTime minDate, DateTime maxDate, bool includeTime)
        {
            Records = records;
            RecordName = recordName;
            MinDate = minDate;
            MaxDate = maxDate;
            IncludeTime = includeTime;
        }
    }
}
