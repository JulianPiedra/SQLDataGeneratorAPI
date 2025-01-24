using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Logic.GenerationUtils;

namespace SqlDataGenerator.Logic.GenerationLogic
{
    public class CountryGeneration : ICountryGeneration
    {
        private readonly SQLGeneratorContext Context;
        public CountryGeneration(SQLGeneratorContext context)
        {
            Context = context;
        }
        public async Task<BusinessLogicResponse> GenerateCountry(Record records)
        {
            try
            {
                var randomCountries = await FetchFromDatabase.FetchObjectListFromDatabase(
                                            records.Records,
                                            "country",
                                            Context.Country,
                                            f => f.CountryName);

                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomCountries };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }
        }
        public async Task<BusinessLogicResponse> GenerateAlphaCode(Record records)
        {
            try
            {
                var randomAlphaCode = await FetchFromDatabase.FetchObjectListFromDatabase(
                                            records.Records,
                                            "alpha_code",
                                            Context.Country, f => f.AlphaCode);

                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomAlphaCode };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<BusinessLogicResponse> GenerateNumericCode(Record records)
        {
            try
            {
                var randomNumericCode = await FetchFromDatabase.FetchObjectListFromDatabase(
                                            records.Records,
                                            "numeric_code",
                                            Context.Country,
                                            f => f.NumericCode);

                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomNumericCode };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }
        }


    }
}
