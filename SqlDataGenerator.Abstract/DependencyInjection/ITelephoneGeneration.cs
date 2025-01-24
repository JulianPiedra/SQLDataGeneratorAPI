using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface ICityGeneration
    {
        Task<BusinessLogicResponse> GenerateCity(Record records);

    }
}
