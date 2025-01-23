using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using SqlDataGenerator.Abstract;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class EmailGenerationEndpoints
{
    public static void MapEmailGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/email_generation").WithTags("Email Generation");

        group.MapGet("/generate_email", async (
            SQLGeneratorContext db,
            IEmailGeneration emailGeneration,
            [FromHeader] int? records) =>
        {
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await emailGeneration.GenerateEmail(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetEmail")
        .WithOpenApi();

        
    }
}
