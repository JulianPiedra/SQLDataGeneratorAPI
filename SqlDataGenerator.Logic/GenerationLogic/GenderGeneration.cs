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
                var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
                var key = string.IsNullOrEmpty(records.RecordName) ? "gender" : records.RecordName;

                var genders = new List<string>
                {
                    "Male",
                    "Female",
                    "Other",
                    "Prefer not to say"
                };

                var gendersList = new ConcurrentBag<object>();

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
