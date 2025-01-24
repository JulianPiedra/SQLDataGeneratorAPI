using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class EmailGenerationEndpoints
{
    public static void MapEmailGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/email_generation").WithTags("Email Generation");

        group.MapGet("/generate_email", async (
            [FromServices] IEmailGeneration emailGeneration,
            [FromHeader] int? records) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await emailGeneration.GenerateEmail(record);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message= result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetEmail")
        .WithOpenApi();

        
    }
}
