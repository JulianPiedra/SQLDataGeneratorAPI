using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator.Models
{
    public class Record
    {
        public int Records { get; set; }
        public Record()
        {
        }
        public Record(int records)
        {
            Records = records;
        }

        public BusinessLogicResponse ValidateRecords()
        {

            if (Records == 0)
            {
                return new BusinessLogicResponse
                {
                    StatusCode = 400,
                    Message = "Records must be a number greater than 0"
                };
            }
            if (Records > 1000000)
            {
                return new BusinessLogicResponse
                {
                    StatusCode = 400,
                    Message = "Records cannot exceed 1,000,000"
                };
            }
            return new BusinessLogicResponse
            {
                StatusCode = 200,
                Message = string.Empty
            };
        }
    }
}
