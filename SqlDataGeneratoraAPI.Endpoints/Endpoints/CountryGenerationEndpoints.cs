using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class CountryGenerationEndpoints
{
    public static void MapCountryGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/country_generation").WithTags("Country Generation");

        group.MapGet("/generate_country", async (
            [FromServices] ICountryGeneration countryGeneration,
            [FromQuery] int? records,
            [FromQuery] string? record_name = null) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0,record_name);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await countryGeneration.GenerateCountry(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json((new { Message= result.Message }, statusCode: result.StatusCode))
            };
        })
        .WithName("GetCountries")
        .WithOpenApi();

        group.MapGet("/generate_alpha_code", async (
            [FromServices] ICountryGeneration countryGeneration,
            [FromQuery] int? records,
            [FromQuery] string ? record_name = null) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0, record_name);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await countryGeneration.GenerateAlphaCode(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json((new { Message= result.Message }, statusCode: result.StatusCode))
            };
        })
        .WithName("GetAlphaCodes")
        .WithOpenApi();

        group.MapGet("/generate_numeric_code", async (
            [FromServices] ICountryGeneration countryGeneration,
            [FromQuery] int? records,
            [FromQuery] string ? record_name = null) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0, record_name);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await countryGeneration.GenerateNumericCode(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message= result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetNumericCodes")
        .WithOpenApi();
    }
}
