using SqlDataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator.Abstract.DependencyInjection
{
    public interface IGenderGeneration
    {
        Task<BusinessLogicResponse> GenerateGender(Record records);
    }
}
