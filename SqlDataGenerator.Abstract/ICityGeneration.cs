using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract
{
    public interface ICityGeneration
    {
        Task<BusinessLogicResponse> GenerateCity(int? records);

    }
}
