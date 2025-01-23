using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using SqlDataGenerator.Abstract;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class GenderGenerationEndpoints
{
    public static void MapGenderGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/gender_generation").WithTags("Gender Generation");

        group.MapGet("/generate_gender", async (
            IGenderGeneration genderGeneration,
            [FromHeader] int? records) =>
        {
            var valiteRecords = RecordsValidator.ValidateRecords(records);
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await genderGeneration.GenerateGender(records);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        })
        .WithName("GetGender")
        .WithOpenApi();


    }
}
