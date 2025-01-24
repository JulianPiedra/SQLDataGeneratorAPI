using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Logic.GenerationUtils;

namespace SqlDataGenerator.Logic.GenerationLogic
{
    public class CityGeneration : ICityGeneration
    {
        private readonly SQLGeneratorContext Context;
        public CityGeneration(SQLGeneratorContext context)
        {
            Context = context;
        }
        public async Task<BusinessLogicResponse> GenerateCity(Record records)
        {
            try
            {
                var randomCountries = await FetchFromDatabase.FetchObjectListFromDatabase(
                                            records.Records,
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
