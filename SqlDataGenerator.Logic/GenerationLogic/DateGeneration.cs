using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Logic.GenerationUtils;
using SqlDataGenerator.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SqlDataGenerator.Logic.GenerationLogic
{
    public class DateGeneration : IDateGeneration
    {
        public async Task<BusinessLogicResponse> GenerateDate(DateConfig dateConfig)
        {

            try
            {
                var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
                var dateList = new ConcurrentBag<object>();

                await Task.WhenAll(
                    Enumerable.Range(0, dateConfig.Records).Select(async _ =>
                    {
                        var pickedDate = RandomDataGeneration.GenerateRandomDate(dateConfig.MinDate, dateConfig.MaxDate, dateConfig.IncludeTime, random.Value);
                        var key = string.IsNullOrEmpty(dateConfig.RecordName) ? "date" : dateConfig.RecordName;
                        dateList.Add(new Dictionary<string, object> { { key, pickedDate } });

                    })
                );

                return new BusinessLogicResponse
                {
                    StatusCode = 200,
                    ObjectResponse = dateList.ToList()
                };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }
    }
}
