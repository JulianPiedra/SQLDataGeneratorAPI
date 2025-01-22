using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Abstract;
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
                IIdGeneration idGeneration,
                [FromHeader] int? records,
                [FromHeader] int? lenght,
                [FromHeader] bool? is_integer = false,
                [FromHeader] bool? has_letters = false) =>
            {
                var valiteRecords = RecordsValidator.ValidateRecords(records);
                if (valiteRecords.StatusCode != 200)
                {
                    return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
                }
                if (lenght == null)
                {
                    return Results.BadRequest(new { Message = "Length is required" });
                }

                if (has_letters!.Value && is_integer!.Value)
                {
                    return Results.BadRequest(new { Message = "Records can't be casted as integer when they have letters" });
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
                    is_integer!.Value,
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
