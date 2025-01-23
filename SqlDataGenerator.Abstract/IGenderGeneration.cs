using SqlDataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator.Abstract
{
    public interface IGenderGeneration
    {
        Task<BusinessLogicResponse> GenerateGender(int? records);
    }
}
