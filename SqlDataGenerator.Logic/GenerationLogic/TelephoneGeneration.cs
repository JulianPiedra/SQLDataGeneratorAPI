using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Logic.GenerationUtils;

namespace SqlDataGenerator.Logic.GenerationLogic
{
    public class TelephoneGeneration : ITelephoneGeneration
    {
        private readonly SQLGeneratorContext Context;
        public TelephoneGeneration(SQLGeneratorContext context)
        {
            Context = context;
        }
        public async Task<BusinessLogicResponse> GenerateTelephone(TelephoneConfig telephoneConfig)
        {
            try
            {
                // If the record name is not provided, use the default name "telephone"
                var key = string.IsNullOrEmpty(telephoneConfig.RecordName) ? "telephone" : telephoneConfig.RecordName;

                string allowedChars = "0123456789";

                // The Random class is not thread-safe, so we need to create a new instance for each thread
                var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
                dynamic telephoneList = null;

                // Use ConcurrentBag to store the generated numbers in a thread-safe collection
                var telephoneGeneration = new ConcurrentBag<object>();

                // Generate data concurrently with the given parameters
                await Task.WhenAll(
                    Enumerable.Range(0, telephoneConfig.Records).Select(async _ =>
                    {
                        var telephoneNumber =
                            RandomDataGeneration.GenerateRandomData(
                                telephoneConfig.Length,
                                allowedChars,
                                random.Value,
                                true
                            );
                        telephoneGeneration.Add(telephoneNumber);
                    })

                );

                //Fetch and add country codes if needed
                if (telephoneConfig.IncludeCode)
                {
                    var randomNumericCode =
                        await FetchFromDatabase.FetchStringListFromDatabase(
                            telephoneConfig.Records,
                            Context.Country,
                            f => f.NumericCode
                        );
                    telephoneList = telephoneGeneration.ToList().Zip(randomNumericCode, (telephone, numCode) =>
                        new Dictionary<string, object>
                        {
                            {key, $"{numCode} {telephone}" }
                        }).ToList();
                }
                else
                {
                    telephoneList = telephoneGeneration
                        .Select(telephone => new Dictionary<string, object> { { key, telephone } })
                        .ToList();
                }

                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = telephoneList };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }
        }



    }
}
