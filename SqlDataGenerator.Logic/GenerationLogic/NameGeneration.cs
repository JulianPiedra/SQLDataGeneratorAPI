using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SqlDataGenerator.Abstract.DependencyInjection;

namespace SqlDataGenerator.Logic.GenerationLogic
{
    public class NameGeneration : INameGeneration
    {
        private readonly SQLGeneratorContext Context;
        private readonly FetchFromDatabase FetchFromDatabase;
        public NameGeneration(SQLGeneratorContext context, FetchFromDatabase fetchFromDatabase)
        {
            Context = context;
            FetchFromDatabase = fetchFromDatabase;
        }
        public async Task<BusinessLogicResponse> GenerateWholeNames(Record records)
        {
            try
            {
                // Start the tasks concurrently with their own DbContext instance
                var randomFirstNames = await FetchFromDatabase.FetchStringListFromDatabase(
                            records.Records,
                            Context.FirstName,
                            f => f.FirstName1);


                var randomLastNames = await FetchFromDatabase.FetchStringListFromDatabase(
                            records.Records,
                            Context.LastName,
                            f => f.LastName1);

                // Combine the results of both tasks
                var result = randomFirstNames.Zip(randomLastNames, (firstName, lastName) => new
                {
                    whole_name = $"{firstName} {lastName}"
                }).ToList();

                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = result };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }

        }
        public async Task<BusinessLogicResponse> GenerateFirstNames(Record records)
        {
            try
            {
                var randomFirstNames = await FetchFromDatabase.FetchObjectListFromDatabase(
                                            records.Records,
                                            "first_name",
                                            Context.FirstName,
                                            f => f.FirstName1);
                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomFirstNames };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<BusinessLogicResponse> GenerateLastNames(Record records)
        {
            try
            {
                var randomLastNames = await FetchFromDatabase.FetchObjectListFromDatabase(
                                            records.Records,
                                            "last_name",
                                            Context.LastName,
                                            f => f.LastName1);
                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomLastNames };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }
        }



    }
}
