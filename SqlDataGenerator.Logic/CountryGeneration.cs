using SqlDataGenerator.Abstract;
using SqlDataGenerator.Models;
using System.Text;
using System.Collections.Concurrent;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace SqlDataGenerator.Logic
{
    public class CountryGeneration : ICountryGeneration
    {
        private readonly SQLGeneratorContext Context;
        private readonly FetchFromDatabase FetchFromDatabase;
        public CountryGeneration(SQLGeneratorContext context, FetchFromDatabase fetchFromDatabase)
        {
            Context = context;
            FetchFromDatabase = fetchFromDatabase;
        }
        public async Task<BusinessLogicResponse> GenerateCountry(int? records)
        {
            try
            {
                var randomCountries = await FetchFromDatabase.FetchObjectListFromDatabase<Country>(
                                            records,
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
        public async Task<BusinessLogicResponse> GenerateAlphaCode(int? records)
        {
            try
            {
                var randomAlphaCode = await FetchFromDatabase.FetchObjectListFromDatabase<Country>(
                                            records,
                                            "alpha_code",
                                            Context.Country, f => f.AlphaCode);

                return new BusinessLogicResponse { StatusCode = 200, ObjectResponse = randomAlphaCode };
            }
            catch (Exception ex)
            {
                return new BusinessLogicResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<BusinessLogicResponse> GenerateNumericCode(int? records)
        {
            try
            {
                var randomNumericCode = await FetchFromDatabase.FetchObjectListFromDatabase<Country>(
                                            records,
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
