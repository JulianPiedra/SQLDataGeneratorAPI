using SqlDataGenerator.Abstract;
using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;

namespace SqlDataGenerator.Logic
{
    public class IdGeneration : IIdGeneration
    {
        public async Task<BusinessLogicResponse> GenerateIds(IdNumberConfig idNumberConfig)
        {
            try
            {
                var random = new Random();
                var allowedChars = "0123456789";
                if (idNumberConfig.HasLetters)
                {
                    allowedChars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                }

                var uniqueIds = new ConcurrentBag<IdNumber>();  // Thread-safe collection
                var generatedIds = new HashSet<string>(); // To ensure no duplicates
                

                // Use Parallel.For to generate IDs concurrently for large numbers of records
                await Task.WhenAll(
                    Enumerable.Range(0, idNumberConfig.Records).Select(async _ =>
                    {

                        string generatedId;
                        do
                        {
                            generatedId = GenerateRandomId(idNumberConfig.Lenght, allowedChars, random);
                        } while (!generatedIds.Add(generatedId));  // Ensure uniqueness

                        uniqueIds.Add(new IdNumber { Id = idNumberConfig.IsInteger ? Int64.Parse(generatedId) : generatedId });
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

        private string GenerateRandomId(int length, string allowedChars, Random random)
        {
            // Use StringBuilder to construct the ID efficiently
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(allowedChars[random.Next(allowedChars.Length)]);
            }

            return stringBuilder.ToString();
        }
    }
}
