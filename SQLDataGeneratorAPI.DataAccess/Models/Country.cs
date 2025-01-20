using System;
using System.Collections.Generic;

namespace SQLDataGeneratorAPI.DataAccess.Models;

public partial class Country
{
    public string AlphaCode { get; set; } = null!;

    public string NumericCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;
}
