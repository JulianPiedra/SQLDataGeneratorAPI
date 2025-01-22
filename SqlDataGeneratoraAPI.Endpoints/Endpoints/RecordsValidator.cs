using SqlDataGenerator.Models;

namespace SqlDataGeneratorAPI.Endpoints.Endpoints
{
    public static class RecordsValidator
    {
        public static BusinessLogicResponse ValidateRecords(int? records)
        {

            if (records == 0)
            {
                return new BusinessLogicResponse
                {
                    StatusCode = 400,
                    Message = "Records can't be 0"
                };
            }

            if (string.IsNullOrEmpty(records.ToString()) )
            {
                return new BusinessLogicResponse
                {
                    StatusCode = 400,
                    Message = "Records must be provided"
                };
            }

            if (records > 1000000)
            {
                return new BusinessLogicResponse
                {
                    StatusCode = 400,
                    Message = "Records cannot exceed 1,000,000 and Length cannot exceed 40"
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

