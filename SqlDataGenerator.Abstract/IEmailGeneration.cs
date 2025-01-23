using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract
{
    public interface IEmailGeneration
    {
        Task<BusinessLogicResponse> GenerateEmail(int? records);

    }
}
