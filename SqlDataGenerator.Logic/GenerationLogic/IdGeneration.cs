using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SqlDataGenerator.Logic.GenerationUtils;
using SqlDataGenerator.Abstract.DependencyInjection;
using System.Numerics;

namespace SqlDataGenerator.Logic.GenerationLogic
{
    public class IdGeneration : IIdGeneration
    {
        public async Task<BusinessLogicResponse> GenerateIds(IdNumberConfig idNumberConfig)
        {
            try
            {
                // If the record name is not provided, use the default name "id"
                var key = string.IsNullOrEmpty(idNumberConfig.RecordName) ? "id" : idNumberConfig.RecordName;

                // The Random class is not thread-safe, so we need to create a new instance for each thread
                var random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
                var allowedChars = "0123456789";

                if (idNumberConfig.HasLetters)
                {
                    allowedChars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                }

                // Use ConcurrentBag to store the generated numbers in a thread-safe collection
                var uniqueIds = new ConcurrentBag<object>();

                // Hashset to ensure uniqueness of generated IDs
                var generatedIds = new HashSet<string>();


                // Generate data concurrently with the given parameters
                await Task.WhenAll(
                    Enumerable.Range(0, idNumberConfig.Records).Select(async _ =>
                    {

                        string generatedId;
                        do
                        {
                            generatedId = RandomDataGeneration.GenerateRandomData(idNumberConfig.Length, allowedChars, random.Value);
                        } while (!generatedIds.Add(generatedId));  // Ensure uniqueness                      

                        uniqueIds.Add(new Dictionary<string, object>
                        {
                            { key, generatedId }
                        });
                    })
                );

                return new BusinessLogicResponse
                {
                    StatusCode = 200,
                    ObjectResponse = uniqueIds.ToList()
                };

            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse(500, ex.Message);
            }
        }


    }
}
