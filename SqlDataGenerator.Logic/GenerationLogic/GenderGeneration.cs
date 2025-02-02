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
    public class GenderGeneration : IGenderGeneration
    {
        public async Task<BusinessLogicResponse> GenerateGender(Record records)
        {

            try
            {
                // If the record name is not provided, use the default name "gender"
                var key = string.IsNullOrEmpty(records.RecordName) ? "gender" : records.RecordName;

                // The Random class is not thread-safe, so we need to create a new instance for each thread
                var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
                
                var genders = new List<string>
                {
                    "Male",
                    "Female",
                    "Other",
                    "Prefer not to say"
                };

                // Use ConcurrentBag to store the generated numbers in a thread-safe collection
                var gendersList = new ConcurrentBag<object>();

                // Generate data concurrently with the given parameters
                await Task.WhenAll(
                    Enumerable.Range(0, records.Records).Select(async _ =>
                    {
                        var pickedGender = RandomDataGeneration.PickRandomData(genders, random.Value);
                        gendersList.Add(new Dictionary<string, object> { { key, pickedGender } });
                    })
                );

                return new BusinessLogicResponse
                {
                    StatusCode = 200,
                    ObjectResponse = gendersList.ToList()
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
