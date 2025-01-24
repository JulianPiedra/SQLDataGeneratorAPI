using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface IDateGeneration
    {
        Task<BusinessLogicResponse> GenerateDate(DateConfig dateConfig);
    }
}
