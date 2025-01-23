using SqlDataGenerator.Abstract;
using SqlDataGenerator.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlDataGenerator.Logic
{
    public class GenderGeneration : IGenderGeneration
    {
        public async Task<BusinessLogicResponse> GenerateGender(int? records)
        {
            
            try
            {
                var random = new Random();
                var genders = new List<string>
                {
                    "Male",
                    "Female",
                    "Other",
                    "Prefer not to say"
                };

                var gendersList = new ConcurrentBag<object>();

                await Task.WhenAll(
                    Enumerable.Range(0, records.Value).Select(async _ =>
                    {
                        var pickedGender = RandomDataGeneration.PickRandomData(genders, random);
                        gendersList.Add(new { gender = pickedGender });
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
                    ObjectResponse = ex.Message
                };
            }
        }
    }
}
