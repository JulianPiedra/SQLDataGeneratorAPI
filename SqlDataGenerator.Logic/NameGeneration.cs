using SqlDataGenerator.Abstract;
using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace SqlDataGenerator.Logic
{
    public class NameGeneration : INameGeneration
    {
        private readonly SQLGeneratorContext db;

        public NameGeneration(SQLGeneratorContext db)
        {
            this.db = db;
        }
        public Task<BusinessLogicResponse> GenerateWholeNames(int? records)
        {
            try
            {
                var randomFirstNames = ListFirstNames(records).Result;
                var randomLastNames = ListLastNames(records).Result;
                var result = randomFirstNames.Zip(randomLastNames, (firstName, lastName) => new
                {
                    whole_name = $"{firstName} {lastName}"
                }).ToList();
                return Task.FromResult(new BusinessLogicResponse { StatusCode = 200, ObjectResponse = result });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new BusinessLogicResponse { StatusCode = 500, Message = ex.Message });
            }
        }
        public Task<BusinessLogicResponse> GenerateFirstNames(int? records)
        {
            try 
            {
                var randomFirstNames = ListFirstNames(records).Result;
                return Task.FromResult(new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomFirstNames });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new BusinessLogicResponse { StatusCode=500, Message = ex.Message });
            }
        }

        public Task<BusinessLogicResponse> GenerateLastNames(int? records)
        {
            try
            {
                var randomLastNames = ListLastNames(records).Result;
                return Task.FromResult(new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomLastNames });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new BusinessLogicResponse { StatusCode = 500, Message = ex.Message });
            }
        }

        
        private async Task<List<string>>  ListFirstNames(int? records) {
            var randomFirstNames = await db.FirstName
                .OrderBy(r => Guid.NewGuid()) 
                .Take(records!.Value) 
                .Select(f => f.FirstName1)
                .ToListAsync();

            while (randomFirstNames.Count < records.Value)
            {
                randomFirstNames.AddRange(randomFirstNames.Take(records.Value - randomFirstNames.Count));
            }
            return randomFirstNames;
        }
        private async Task<List<string>> ListLastNames(int? records)
        {
            var randomLastNames = await db.LastName
            .OrderBy(r => Guid.NewGuid())
            .Take(records!.Value) 
            .Select(l => l.LastName1)
            .ToListAsync();

            while (randomLastNames.Count < records.Value)
            {
                randomLastNames.AddRange(randomLastNames.Take(records.Value - randomLastNames.Count));
            }
            return randomLastNames;
        }
    }
}
