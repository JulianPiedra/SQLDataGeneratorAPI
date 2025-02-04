using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
using Microsoft.AspNetCore.WebUtilities;
using SQLDataGeneratorAPI.Endpoints.Endpoints.Documentation;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class TelephoneGenerationEndpoints
{
    public static void MapTelephoneGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/telephone_generation").WithTags("Telephone Generation");

        group.MapGet("/generate_telephone", async (
            [FromServices] ITelephoneGeneration telephoneGeneration,
            [FromQuery] int? records,
            [FromQuery] int? length,
            [FromQuery] bool? include_code = false,
            [FromQuery] string? record_name = null
        ) =>
        {
            TelephoneConfig telephoneConfig = new TelephoneConfig(
                records.HasValue ? records.Value : 0,
                record_name,
                length.HasValue && length > 0 ? length.Value : 10,
                include_code.Value
            );

            var valiteRecords = telephoneConfig.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await telephoneGeneration.GenerateTelephone(telephoneConfig);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetTelephone")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Generates a list of random telephone numbers.";
            operation.Description = "Generates telephone numbers of a specified length with an optional country code.";
            operation.Parameters[0].Description = "The number of records to generate.";
            operation.Parameters[1].Description = "The length of the telephone numbers (default: 10).";
            operation.Parameters[2].Description = "Boolean flag indicating whether to include a country code (default: false).";
            operation.Parameters[3].Description = "The name of the record object (optional).";
            operation.Responses["200"] = GetDocumentation.Return200("A list of generated telephone numbers.", "telephone", "+1234567890");
            operation.Responses["400"] = GetDocumentation.Return400();
            operation.Responses["401"] = GetDocumentation.Return401();
            operation.Responses["500"] = GetDocumentation.Return500();
            return operation;
        });


    }
}
