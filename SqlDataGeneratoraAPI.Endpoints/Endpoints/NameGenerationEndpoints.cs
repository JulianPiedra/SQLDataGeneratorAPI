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
using SQLDataGeneratorAPI.Endpoints.Endpoints.Documentation;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class NameGenerationEndpoints
{
    public static void MapNameGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/name_generation").WithTags("Name Generation");

        group.MapGet("/generate_whole_name", async (
            [FromServices] INameGeneration nameGeneration,
            [FromQuery] int? records,
            [FromQuery] string? record_name = null
        ) =>
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
                        _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
                    };
                })
        .WithName("GetWholeNames")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Generates a list of whole names (first name + last name).";
            operation.Description = "Generates a list of whole names based on the specified records and record name.";
            operation.Parameters[0].Description = "The number of records to generate.";
            operation.Parameters[1].Description = "The name of the record object (optional).";
            operation.Responses["200"] = GetDocumentation.Return200("A list of generated whole names.", "name", "John Doe");
            operation.Responses["400"] = GetDocumentation.Return400();
            operation.Responses["401"] = GetDocumentation.Return401();
            operation.Responses["500"] = GetDocumentation.Return500();
            return operation;
        });

        group.MapGet("/generate_first_name", async (
            [FromServices] INameGeneration nameGeneration,
            [FromQuery] int? records,
            [FromQuery] string? record_name = null
        ) =>
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
                        _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
                    };
                })
        .WithName("GetFirstNames")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Generates a list of first names.";
            operation.Description = "Generates a list of first names based on the specified records and record name.";
            operation.Parameters[0].Description = "The number of records to generate.";
            operation.Parameters[1].Description = "The name of the record object (optional).";
            operation.Responses["200"] = GetDocumentation.Return200("A list of generated first names.", "first_name", "John");
            operation.Responses["400"] = GetDocumentation.Return400();
            operation.Responses["401"] = GetDocumentation.Return401();
            operation.Responses["500"] = GetDocumentation.Return500();
            return operation;
        });

        group.MapGet("/generate_last_name", async (
            [FromServices] INameGeneration nameGeneration,
            [FromQuery] int? records,
            [FromQuery] string? record_name = null
        ) =>
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
                        _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
                    };
                })
        .WithName("GetLastNames")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Generates a list of last names.";
            operation.Description = "Generates a list of last names based on the specified records and record name.";
            operation.Parameters[0].Description = "The number of records to generate.";
            operation.Parameters[1].Description = "The name of the record object (optional).";
            operation.Responses["200"] = GetDocumentation.Return200("A list of generated last names.", "last_name", "Doe");
            operation.Responses["400"] = GetDocumentation.Return400();
            operation.Responses["401"] = GetDocumentation.Return401();
            operation.Responses["500"] = GetDocumentation.Return500();
            return operation;
        });



    }
}
