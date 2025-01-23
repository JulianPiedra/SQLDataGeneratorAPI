using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface IIdGeneration
    {
        Task<BusinessLogicResponse> GenerateIds(IdNumberConfig idNumberConfig);

    }
}
