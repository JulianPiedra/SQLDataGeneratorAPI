using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using SqlDataGenerator.Abstract;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class CityGenerationEndpoints
{
    public static void MapCityGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/city_generation").WithTags("City Generation");

        group.MapGet("/generate_city", async (
            SQLGeneratorContext db,
            ICityGeneration cityGeneration,
            [FromHeader] int? records) =>
        {
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await cityGeneration.GenerateCity(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetCity")
        .WithOpenApi();

        
    }
}
