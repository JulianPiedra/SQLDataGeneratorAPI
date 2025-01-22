using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using NuGet.Packaging;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class NameGenerationEndpoints
{
    public static void MapNameGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/name_generation").WithTags("Name Generation");

        group.MapGet("/generate_whole_name", async (
            SQLGeneratorContext db,
            INameGeneration nameGeneration, 
            [FromHeader] int? records) =>
        {
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await nameGeneration.GenerateWholeNames(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetWholeNames")
        .WithOpenApi();

        group.MapGet("/generate_first_name", async (
            SQLGeneratorContext db,
            INameGeneration nameGeneration,
            [FromHeader] int? records) =>
        {
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }
            var result = await nameGeneration.GenerateFirstNames(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetFirstNames")
        .WithOpenApi();

        group.MapGet("/generate_last_name", async (
            SQLGeneratorContext db,
            INameGeneration nameGeneration,
            [FromHeader] int? records) =>
        {
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }
            var result = await nameGeneration.GenerateLastNames(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetLastNames")
        .WithOpenApi();
    }
}
