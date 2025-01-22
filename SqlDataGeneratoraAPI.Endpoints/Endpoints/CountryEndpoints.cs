using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using SqlDataGenerator.Abstract;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class CountryGenerationEndpoints
{
    public static void MapCountryGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/country_generation").WithTags("Country Generation");

        group.MapGet("/generate_country", async (
            SQLGeneratorContext db,
            ICountryGeneration countryGeneration,
            [FromHeader] int? records) =>
        {
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message}, statusCode:valiteRecords.StatusCode );
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
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
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
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
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
