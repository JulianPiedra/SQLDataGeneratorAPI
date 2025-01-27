using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Abstract;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class CityGenerationEndpoints
{
    public static void MapCityGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/city_generation").WithTags("City Generation");

        group.MapGet("/generate_city", async (
            [FromServices] ICityGeneration cityGeneration,
            [FromQuery] int? records) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await cityGeneration.GenerateCity(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message= result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetCity")
        .WithOpenApi();

        
    }
}
