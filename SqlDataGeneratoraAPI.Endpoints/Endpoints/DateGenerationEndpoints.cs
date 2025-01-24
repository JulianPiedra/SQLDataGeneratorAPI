using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using SQLDataGeneratorAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SqlDataGenerator.Logic;
using SqlDataGenerator.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
namespace SqlDataGeneratorAPI.Endpoints.Endpoints;

public static class DateGenerationEndpoints
{
    public static void MapDateGenerationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/date_generation").WithTags("Date Generation");

        group.MapGet("/generate_date", async (
            [FromServices] IDateGeneration dateGeneration,
            [FromHeader] int? records,
            [FromHeader] DateTime? min_date,
            [FromHeader] DateTime? max_date,
            [FromHeader] bool? include_time = false) =>
        {
            min_date = min_date.HasValue? min_date : DateTime.Parse("1950-01-01");
            max_date = max_date.HasValue? max_date : DateTime.Parse("2060-12-31");
            DateConfig dateConfig = new DateConfig(
                records.HasValue ? records.Value : 0,
                min_date,
                max_date,
                include_time.Value
            );
            var valiteRecords = dateConfig.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
            }
            if (min_date.Value.CompareTo(max_date.Value) == 1)
            {
                return Results.BadRequest(new { Message = "Start date cannot be after the end date." });
            }
            


            var result = await dateGeneration.GenerateDate(dateConfig);
            return result.StatusCode switch
            {
                200 => Results.Ok(result.ObjectResponse),
                _ => Results.Json(new { Message = result.Message }, statusCode: result.StatusCode)
            };
        })
        .WithName("GetDate")
        .WithOpenApi();


    }
}
