using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
using SQLDataGeneratorAPI.Endpoints.Endpoints.Documentation;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class NumberGenerationEndpoints
{
    public static void MapNumberGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/number_generation").WithTags("Number Generation");

        group.MapGet("/generate_number", async (
            [FromServices] INumberGeneration numberGeneration,
            [FromQuery] int? records,
            [FromQuery] int? min_value,
            [FromQuery] int? max_value,
            [FromQuery] string? record_name = null
        ) =>
        {
            if (min_value > max_value)
            {
                return Results.BadRequest(new { Message = "Minimum value cannot be greater than the maximum value." });
            }
            min_value = min_value.HasValue ? min_value : 1;
            max_value = max_value.HasValue ? max_value : 100000000;

            NumberConfig numberConfig = new NumberConfig(
                records.HasValue ? records.Value : 0,
                record_name,
                min_value.Value,
                max_value.Value
            );

            var valiteRecords = numberConfig.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await numberGeneration.GenerateNumber(numberConfig);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetNumber")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Generates a list of random numbers.";
            operation.Description = "Generates a list of numbers within the specified range.";
            operation.Parameters[0].Description = "The number of records to generate.";
            operation.Parameters[1].Description = "The minimum value for generated numbers (default: 1).";
            operation.Parameters[2].Description = "The maximum value for generated numbers (default: 100,000,000).";
            operation.Parameters[3].Description = "The name of the record object (optional).";
            operation.Responses["200"] = GetDocumentation.Return200("A list of generated numbers.", "number", "12345");
            operation.Responses["400"] = GetDocumentation.Return400();
            operation.Responses["401"] = GetDocumentation.Return401();
            operation.Responses["500"] = GetDocumentation.Return500();
            return operation;
        });


    }
}
