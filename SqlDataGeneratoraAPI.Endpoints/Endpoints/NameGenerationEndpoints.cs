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

        group.MapGet("", async (
            SQLGeneratorContext db,
            INameGeneration nameGeneration, 
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

            var result = await nameGeneration.GenerateWholeNames(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetWholeNames")
        .WithOpenApi();

        group.MapGet("/first_name", async (
            SQLGeneratorContext db,
            INameGeneration nameGeneration,
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
            var result = await nameGeneration.GenerateFirstNames(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetFirstNames")
        .WithOpenApi();

        group.MapGet("/last_name", async (
            SQLGeneratorContext db,
            INameGeneration nameGeneration,
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
