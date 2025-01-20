using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SQLDataGeneratorAPI.DataAccess.Models;

public partial class LastName
{
    [JsonPropertyName("last_name")]

    public string LastName1 { get; set; } = null!;
}
