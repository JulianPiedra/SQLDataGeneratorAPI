using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface INumberGeneration
    {
        Task<BusinessLogicResponse> GenerateNumber(NumberConfig numberConfig);
    }
}
