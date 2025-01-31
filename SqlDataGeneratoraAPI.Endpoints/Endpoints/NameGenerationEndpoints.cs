using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using NuGet.Packaging;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class NameGenerationEndpoints
{
    public static void MapNameGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/name_generation").WithTags("Name Generation");

        group.MapGet("/generate_whole_name", async (
            [FromServices] INameGeneration nameGeneration,
            [FromQuery] int? records,
            [FromQuery] string ? record_name = null) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0, record_name);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await nameGeneration.GenerateWholeNames(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message= result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetWholeNames")
        .WithOpenApi();

        group.MapGet("/generate_first_name", async (
            [FromServices] INameGeneration nameGeneration,
            [FromQuery] int? records,
            [FromQuery] string? record_name = null) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0, record_name);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }
            var result = await nameGeneration.GenerateFirstNames(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message= result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetFirstNames")
        .WithOpenApi();

        group.MapGet("/generate_last_name", async (
            [FromServices] INameGeneration nameGeneration,
            [FromQuery] int? records,
            [FromQuery] string ? record_name = null) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0, record_name);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }
            var result = await nameGeneration.GenerateLastNames(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message= result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetLastNames")
        .WithOpenApi();
    }
}
