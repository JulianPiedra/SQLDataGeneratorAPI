using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SqlDataGenerator.Endpoints;
using SQLDataGeneratorAPI.DataAccess.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Net.WebRequestMethods;
using SqlDataGeneratorAPI.Endpoints.Endpoints;
using SqlDataGenerator.Logic.GenerationLogic;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SQLGeneratorContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add security definition for API key
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X_API_KEY", // The name of the header where the API key is passed
        Type = SecuritySchemeType.ApiKey,
        Description = "Unauthorized. API key is missing or invalid."
        
    });
    // Apply the security definition globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey",
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IIdGeneration, IdGeneration>();
builder.Services.AddScoped<INameGeneration, NameGeneration>();
builder.Services.AddScoped<ICountryGeneration, CountryGeneration>();
builder.Services.AddScoped<ICityGeneration, CityGeneration>();
builder.Services.AddScoped<IGenderGeneration, GenderGeneration>();
builder.Services.AddScoped<IEmailGeneration, EmailGeneration>();
builder.Services.AddScoped<IDateGeneration, DateGeneration>();


builder.Services.AddAuthentication("ApiKey")
    .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>(
        "ApiKey",
        opts => opts.ApiKey = builder.Configuration.GetValue<string>("ApiKey") ?? "No key provided"
    );

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapIdGenerationEndpoints();
app.MapCountryGenerationEndpoints();
app.MapNameGenerationEndpoints();
app.MapCityGenerationEndpoints();
app.MapGenderGenerationEndpoints();
app.MapEmailGenerationEndpoints();
app.MapDateGenerationEndpoints();
app.Run();


