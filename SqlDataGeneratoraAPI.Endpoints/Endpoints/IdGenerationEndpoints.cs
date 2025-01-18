using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Abstract;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;

namespace SqlDataGenerator.Endpoints
{
    public static class IdGenerationEndpoints
    {
        public static void MapIdGenerationEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/").WithTags("IdGeneration");

            group.MapGet("/generate_ids",async (
                IIdGeneration idGeneration,
                [FromHeader] int? records,
                [FromHeader] int? lenght,
                [FromHeader] bool? is_integer = false, 
                [FromHeader] bool? has_letters = false) =>
            {
                if (records == null || lenght == null)
                {
                    return Results.BadRequest(new { Message = "Records and Length must be provided" } );
                }
                if (records > 1000000 || lenght > 40)
                {
                    return Results.BadRequest(new { Message = "Records cannot exceed 1,000,000 and Length cannot exceed 40" });
                }
                if (has_letters.Value && is_integer.Value) {
                    return Results.BadRequest(new { Message = "Records can't be casted as integer when they have letters" });
                }

                IdNumberConfig idNumberConfig = new IdNumberConfig(
                    records.Value,
                    lenght.Value,
                    is_integer.Value,
                    has_letters.Value
                );
                var result = await idGeneration.GenerateIds(idNumberConfig);
                return result.StatusCode switch
                {
                    200 => Results.Ok(result.ObjectResponse),
                    _ => Results.Json(result.Message, statusCode: result.StatusCode)
                };
            }).RequireAuthorization();


        }
    }
}
