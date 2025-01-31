using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Logic.GenerationUtils;

namespace SqlDataGenerator.Logic.GenerationLogic
{
    public class NameGeneration : INameGeneration
    {
        private readonly SQLGeneratorContext Context;
        public NameGeneration(SQLGeneratorContext context)
        {
            Context = context;
        }
        public async Task<BusinessLogicResponse> GenerateWholeNames(Record records)
        {

            try
            {
                var key = string.IsNullOrEmpty(records.RecordName) ? "whole_name" : records.RecordName;

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
                var result = randomFirstNames.Zip(randomLastNames, (firstName, lastName) => new Dictionary<string, object>
                {
                    {key , $"{firstName} {lastName}"}
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
                                            string.IsNullOrEmpty(records.RecordName) ? "first_name" : records.RecordName,
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
                                            string.IsNullOrEmpty(records.RecordName) ? "last_name" : records.RecordName,
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
