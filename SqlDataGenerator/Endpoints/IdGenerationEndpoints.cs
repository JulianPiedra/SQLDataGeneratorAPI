using Microsoft.AspNetCore.Authentication;
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

            group.MapGet("/GenerateIds", async (
                IIdGeneration idGeneration,
                [FromHeader] int? records,
                [FromHeader] int? lenght,
                [FromHeader] bool? isInteger = false, 
                [FromHeader] bool? hasLetters = false) =>
            {
                if (records == null || lenght == null)
                {
                    return Results.Conflict(new { Message = "Records and Length must be provided" } );
                }
                if (records > 1000000 || lenght > 40)
                {
                    return Results.Conflict(new { Message = "Records cannot exceed 1,000,000 and Length cannot exceed 40" });
                }
                if (hasLetters.Value && isInteger.Value) {
                    return Results.Conflict(new { Message = "Records can't be casted as integer when they have letters" });
                }

                IdNumberConfig idNumberConfig = new IdNumberConfig(
                    records.Value,
                    lenght.Value,
                    isInteger.Value,
                    hasLetters.Value
                );
                var result = await idGeneration.GenerateIds(idNumberConfig);
                return result.StatusCode switch
                {
                    200 => Results.Ok(result.ObjectResponse),
                    _ => Results.Json(result.Message, statusCode: result.StatusCode)
                };
            });

        }
    }
}
