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
                string allowedChars = "0123456789";
                var key = string.IsNullOrEmpty(telephoneConfig.RecordName) ? "telephone" : telephoneConfig.RecordName;
                var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
                dynamic telephoneList = null;
                var telephoneGeneration = new ConcurrentBag<object>();

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
