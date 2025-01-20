using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SQLDataGeneratorAPI.DataAccess.Models;

public partial class FirstName
{
    [JsonPropertyName("first_name")]
    public string FirstName1 { get; set; } = null!;
}
