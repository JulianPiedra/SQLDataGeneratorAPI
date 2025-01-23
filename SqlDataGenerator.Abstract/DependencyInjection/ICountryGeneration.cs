using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface ICountryGeneration
    {
        Task<BusinessLogicResponse> GenerateCountry(Record records);
        Task<BusinessLogicResponse> GenerateAlphaCode(Record records);
        Task<BusinessLogicResponse> GenerateNumericCode(Record records);

    }
}
