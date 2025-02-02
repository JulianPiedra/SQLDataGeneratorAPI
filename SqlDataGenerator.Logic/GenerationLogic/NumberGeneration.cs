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
                // If the record name is not provided, use the default name "number"
                var key = string.IsNullOrEmpty(numberConfig.RecordName) ? "number" : numberConfig.RecordName;

                // The Random class is not thread-safe, so we need to create a new instance for each thread
                var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

                // Use ConcurrentBag to store the generated numbers in a thread-safe collection
                var numberList = new ConcurrentBag<object>();

                // Generate data concurrently with the given parameters
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
