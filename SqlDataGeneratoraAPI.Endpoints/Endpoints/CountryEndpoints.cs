using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using SqlDataGenerator.Abstract;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class CountryEndpoints
{
    public static void MapCountryGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/country_generation").WithTags(nameof(Country));

        group.MapGet("/generate_country", async (
            SQLGeneratorContext db,
            ICountryGeneration countryGeneration,
            [FromHeader] int? records) =>
        {
            if (records == null)
            {
                return Results.BadRequest(new { Message = "Records must be provided" });
            }
            if (records > 1000000)
            {
                return Results.BadRequest(new { Message = "Records cannot exceed 1,000,000 and Length cannot exceed 40" });
            }

            var result = await countryGeneration.GenerateCountry(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetCountries")
        .WithOpenApi();

        group.MapGet("/generate_alpha_code", async (
            SQLGeneratorContext db,
            ICountryGeneration countryGeneration,
            [FromHeader] int? records) =>
        {
            if (records == null)
            {
                return Results.BadRequest(new { Message = "Records must be provided" });
            }
            if (records > 1000000)
            {
                return Results.BadRequest(new { Message = "Records cannot exceed 1,000,000 and Length cannot exceed 40" });
            }

            var result = await countryGeneration.GenerateAlphaCode(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetAlphaCodes")
        .WithOpenApi();

        group.MapGet("/generate_numeric_code", async (
            SQLGeneratorContext db,
            ICountryGeneration countryGeneration,
            [FromHeader] int? records) =>
        {
            if (records == null)
            {
                return Results.BadRequest(new { Message = "Records must be provided" });
            }
            if (records > 1000000)
            {
                return Results.BadRequest(new { Message = "Records cannot exceed 1,000,000 and Length cannot exceed 40" });
            }

            var result = await countryGeneration.GenerateNumericCode(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetNumericCodes")
        .WithOpenApi();
    }
}
