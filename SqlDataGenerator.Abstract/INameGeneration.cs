using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract
{
    public interface INameGeneration
    {
        Task<BusinessLogicResponse> GenerateWholeNames(int? records);
        Task<BusinessLogicResponse> GenerateFirstNames(int? records);
        Task<BusinessLogicResponse> GenerateLastNames(int? records);


    }
}
