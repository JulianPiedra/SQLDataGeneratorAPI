using SqlDataGenerator.Models;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface ITelephoneGeneration
    {
        Task<BusinessLogicResponse> GenerateTelephone(TelephoneConfig telephoneConfig);

    }
}
