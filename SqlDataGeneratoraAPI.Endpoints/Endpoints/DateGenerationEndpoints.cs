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
            [FromQuery] int? records,
            [FromQuery] DateTime? min_date,
            [FromQuery] DateTime? max_date,
            [FromQuery] bool? include_time = false,
            [FromQuery] string? record_name = null) =>
        {

            min_date = min_date.HasValue ? min_date : DateTime.Parse("1950-01-01");
            max_date = max_date.HasValue ? max_date : DateTime.Parse("2060-12-31");
            if (min_date.Value.CompareTo(max_date.Value) == 1)
            {
                return Results.BadRequest(new { Message = "Start date cannot be after the end date." });
            }
            DateConfig dateConfig = new DateConfig(
                records.HasValue ? records.Value : 0,
                record_name,
                min_date.Value,
                max_date.Value,
                include_time.Value
            );
            var valiteRecords = dateConfig.ValidateRecords();
            if (valiteRecords.StatusCode != 200)
            {
                return Results.Json(new { Message = valiteRecords.Message }, statusCode: valiteRecords.StatusCode);
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
