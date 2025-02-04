using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
using SQLDataGeneratorAPI.Endpoints.Endpoints.Documentation;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class EmailGenerationEndpoints
{
    public static void MapEmailGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/email_generation").WithTags("Email Generation");

        group.MapGet("/generate_email", async (
            [FromServices] IEmailGeneration emailGeneration,
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

            var result = await emailGeneration.GenerateEmail(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetEmail")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Generates a list of email addresses.";
            operation.Description = "Generates a list of email addresses based on the provided records and record name.";
            operation.Parameters[0].Description = "The number of records to generate.";
            operation.Parameters[1].Description = "The name of the record object (optional).";
            operation.Responses["200"] = GetDocumentation.Return200("A list of generated email addresses.", "email", "example@example.com");
            operation.Responses["400"] = GetDocumentation.Return400();
            operation.Responses["401"] = GetDocumentation.Return401();
            operation.Responses["500"] = GetDocumentation.Return500();
            return operation;
        });


    }
}
