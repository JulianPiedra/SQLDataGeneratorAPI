using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract
{
    public interface IIdGeneration
    {
        Task<BusinessLogicResponse> GenerateIds(IdNumberConfig idNumberConfig);

    }
}
