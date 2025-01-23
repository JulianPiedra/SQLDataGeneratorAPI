using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface INameGeneration
    {
        Task<BusinessLogicResponse> GenerateWholeNames(Record records);
        Task<BusinessLogicResponse> GenerateFirstNames(Record records);
        Task<BusinessLogicResponse> GenerateLastNames(Record records);


    }
}
