using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Logic.GenerationUtils;
using SqlDataGenerator.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlDataGenerator.Logic.GenerationLogic
{
    public class NumberGeneration : INumberGeneration
    {
        public async Task<BusinessLogicResponse> GenerateNumber(NumberConfig numberConfig)
        {

            try
            {
                var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
                var numberList = new ConcurrentBag<object>();
                var key = string.IsNullOrEmpty(numberConfig.RecordName) ? "number" : numberConfig.RecordName;

                await Task.WhenAll(
                    Enumerable.Range(0, numberConfig.Records).Select(async _ =>
                    {
                        var pickedNumber = RandomDataGeneration.GenerateRandomNumber(numberConfig.MinValue, numberConfig.MaxValue, random.Value);
                        numberList.Add(new Dictionary<string, object> { { key, pickedNumber } });
                    })
                );

                return new BusinessLogicResponse
                {
                    StatusCode = 200,
                    ObjectResponse = numberList.ToList()
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
