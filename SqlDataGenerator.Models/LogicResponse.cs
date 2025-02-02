using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator.Models
{
    //This class manages responses produced by the logic 
    public class BusinessLogicResponse
    {
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = null!;
        public object ObjectResponse { get; set; } = null!;
        public BusinessLogicResponse()
        {

        }
        public BusinessLogicResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public BusinessLogicResponse(int statusCode, object objectResponse)
        {
            StatusCode = statusCode;
            ObjectResponse = objectResponse;
        }
    }
}
