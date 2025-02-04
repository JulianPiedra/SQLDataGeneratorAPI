using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGeneratorAPI.Endpoints.Endpoints;
using SQLDataGeneratorAPI.DataAccess.Models;
using SQLDataGeneratorAPI.Endpoints.Endpoints.Documentation;

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
                [FromQuery] string? record_name = null
            ) =>
            {
                Record record = new Record(records.HasValue ? records.Value : 0, record_name);
                var valiteRecords = record.ValidateRecords();
                if (length > 40) return Results.BadRequest(new { Message = "Length cannot be greater than 40" });

                // Give a default value to length 
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
            })
            .RequireAuthorization()
            .WithOpenApi(operation =>
            {
                operation.Summary = "Generates a list of unique IDs.";
                operation.Description = "Generates a list of unique IDs based on the specified parameters including the number of records, ID length, and whether IDs should contain letters.";
                operation.Parameters[0].Description = "The number of records to generate.";
                operation.Parameters[1].Description = "The length of each generated ID (must not exceed 40). Defaults to 10 if not provided.";
                operation.Parameters[2].Description = "Whether the generated ID should contain letters (both upper and lower case). Defaults to false.";
                operation.Parameters[3].Description = "The name of the record object (optional).";
                operation.Responses["200"] = GetDocumentation.Return200("A list of generated IDs.", "id", "ABCD1234");
                operation.Responses["400"] = GetDocumentation.Return400();
                operation.Responses["401"] = GetDocumentation.Return401();
                operation.Responses["500"] = GetDocumentation.Return500();
                return operation;
            });


        }
    }
}
