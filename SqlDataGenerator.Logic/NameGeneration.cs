using SqlDataGenerator.Abstract;
using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SqlDataGenerator.Logic
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
        public async Task<BusinessLogicResponse> GenerateWholeNames(int? records)
        {
            try
            {
                // Start the tasks concurrently with their own DbContext instance
                var randomFirstNames = await FetchFromDatabase.FetchStringListFromDatabase<FirstName>(
                            records,
                            Context.FirstName,
                            f => f.FirstName1);


                var randomLastNames = await FetchFromDatabase.FetchStringListFromDatabase<LastName>(
                            records,
                            Context.LastName,
                            f => f.LastName1);

                // Combine the results of both tasks
                var result = randomFirstNames.Zip(randomLastNames, (firstName , lastName) => new
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
        public async Task<BusinessLogicResponse> GenerateFirstNames(int? records)
        {
            try
            {
                var randomFirstNames = await FetchFromDatabase.FetchObjectListFromDatabase<FirstName>(
                                            records,
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

        public async Task<BusinessLogicResponse> GenerateLastNames(int? records)
        {
            try
            {
                var randomLastNames = await FetchFromDatabase.FetchObjectListFromDatabase<LastName>(
                                            records,
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
