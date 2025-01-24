using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGeneratorAPI.Endpoints.Endpoints;
using SQLDataGeneratorAPI.DataAccess.Models;

namespace SqlDataGenerator.Endpoints
{
    public static class IdGenerationEndpoints
    {
        public static void MapIdGenerationEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/id_generation").WithTags("ID Generation");

            group.MapGet("/generate_id", async (
                [FromServices] IIdGeneration idGeneration,
                [FromHeader] int? lenght,
                [FromHeader] int? records,
                [FromHeader] bool? has_letters = false) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0);
            var valiteRecords = record.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }
            if (lenght > 40)
            {
                return Results.BadRequest(new { Message = "Length can't be greater than 40" });
            }
            if (lenght == null)
            {
                return Results.BadRequest(new { Message = "Length is required" });
            }
            if (lenght.Value < 6)
            {
                int characters = has_letters.Value ? 36 : 10;
                int posibleValues = (int)Math.Pow(characters, lenght.Value);
                if (records > posibleValues)
                {
                    return Results.BadRequest(new { Message = "Records exceed the possible values for the given length" });
                }
            }


            IdNumberConfig idNumberConfig = new IdNumberConfig(
                records.Value,
                lenght.Value,
                has_letters.Value
            );
            var result = await idGeneration.GenerateIds(idNumberConfig);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(result.Message, statusCode: result.StatusCode)
            };
        }).RequireAuthorization()
            .WithOpenApi();


        }
    }
}
