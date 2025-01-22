using SqlDataGenerator.Abstract;
using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace SqlDataGenerator.Logic
{
    public class CityGeneration : ICityGeneration
    {
        private readonly SQLGeneratorContext Context;
        private readonly FetchFromDatabase FetchFromDatabase;
        public CityGeneration(SQLGeneratorContext context, FetchFromDatabase fetchFromDatabase)
        {
            Context = context;
            FetchFromDatabase = fetchFromDatabase;
        }
        public async Task<BusinessLogicResponse> GenerateCity(int? records)
        {
            try
            {
                var randomCountries = await FetchFromDatabase.FetchObjectListFromDatabase<City>(
                                            records,
                                            "city",
                                            Context.City,
                                            f => f.CityName);

                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomCountries };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }
        }
       


    }
}
