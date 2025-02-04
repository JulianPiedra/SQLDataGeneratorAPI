using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Abstract;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using SQLDataGeneratorAPI.Endpoints.Endpoints.Documentation;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class CityGenerationEndpoints
{
    public static void MapCityGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/city_generation").WithTags("City Generation");
        group.MapGet("/generate_city", async (
            [FromServices] ICityGeneration cityGeneration,
            [FromQuery] int? records,
            [FromQuery] string? record_name = null
            ) =>
        {
            Record record = new Record(records ?? 0, record_name);
            var validateRecords = record.ValidateRecords();
            if (validateRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = validateRecords.Message }, statusCode: validateRecords.StatusCode);
            }

            var result = await cityGeneration.GenerateCity(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetCity")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Generates a list of cities.";
            operation.Description = "Generates a list of cities.";
            operation.Parameters[0].Description = "The number of records to generate.";
            operation.Parameters[1].Description = "The name of the record object (optional).";
            operation.Responses["200"] = GetDocumentation.Return200("A list of generated cities.", "city","Sample City");
            operation.Responses["400"] = GetDocumentation.Return400();
            operation.Responses["401"] = GetDocumentation.Return401();
            operation.Responses["500"] = GetDocumentation.Return500();
            return operation;
        });
    }
}

