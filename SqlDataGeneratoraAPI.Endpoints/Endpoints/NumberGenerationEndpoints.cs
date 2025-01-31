using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
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
            [FromQuery] string ? record_name = null) =>
        {
            if (min_value < max_value)
            {
                return Results.BadRequest(new { Message = "Minimum value cannot be grater than the maximum value." });
            }
            min_value = min_value.HasValue? min_value : 1;
            max_value = max_value.HasValue? max_value : 100000000;
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
        .WithOpenApi();


    }
}
