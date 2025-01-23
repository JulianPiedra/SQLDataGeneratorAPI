using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface IEmailGeneration
    {
        Task<BusinessLogicResponse> GenerateEmail(Record records);

    }
}
