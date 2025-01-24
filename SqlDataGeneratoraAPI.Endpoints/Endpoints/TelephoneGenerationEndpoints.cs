using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
using Microsoft.AspNetCore.WebUtilities;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class TelephoneGenerationEndpoints
{
    public static void MapTelephoneGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/telephone_generation").WithTags("Telephone Generation");

        group.MapGet("/generate_telephone", async (
            [FromServices] ITelephoneGeneration telephoneGeneration,
            [FromHeader] int? records,
            [FromHeader] bool? include_code = false,
            [FromHeader] int? length = 10) =>
        {
            TelephoneConfig telephoneConfig = new TelephoneConfig(
                records.HasValue ? records.Value : 0, 
                length.Value,
                include_code.Value
                );
            var valiteRecords = telephoneConfig.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }

            var result = await telephoneGeneration.GenerateTelephone(telephoneConfig);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message= result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetTelephone")
        .WithOpenApi();

       
    }
}
