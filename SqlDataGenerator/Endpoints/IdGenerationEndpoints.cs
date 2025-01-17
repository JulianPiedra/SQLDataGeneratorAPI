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
                [FromHeader] bool hasLetters = false) =>
            {
                
                if (records == null || lenght == null) {
                    return Results.BadRequest();
                }
                IdNumberConfig idNumberConfig = new IdNumberConfig (
                    records.Value,
                    lenght.Value,
                    hasLetters
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
