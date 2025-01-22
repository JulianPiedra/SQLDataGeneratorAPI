using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract
{
    public interface ICountryGeneration
    {
        Task<BusinessLogicResponse> GenerateCountry(int? records);
        Task<BusinessLogicResponse> GenerateAlphaCode(int? records);
        Task<BusinessLogicResponse> GenerateNumericCode(int? records);

    }
}
