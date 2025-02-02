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
                [FromQuery] int? records,
                [FromQuery] int? length,
                [FromQuery] bool? has_letters = false,
                [FromQuery] string? record_name = null) =>
        {
            Record record = new Record(records.HasValue ? records.Value : 0, record_name);
            var valiteRecords = record.ValidateRecords();
            
            //Give a default value to length 
            length = length.HasValue && length > 0 ? length.Value : 10;

            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }
            if (length.Value < 6)
            {
                int characters = has_letters.Value ? 36 : 10;
                int posibleValues = (int)Math.Pow(characters, length.Value);
                if (records > posibleValues)
                {
                    return Results.BadRequest(new { Message = "Records exceed the possible values for the given length" });
                }
            }

            IdNumberConfig idNumberConfig = new IdNumberConfig(
                records.Value,
                record_name,
                length.Value,
                has_letters.Value
            );
            var result = await idGeneration.GenerateIds(idNumberConfig);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
            };
        }).RequireAuthorization()
            .WithOpenApi();

        }
    }
}
