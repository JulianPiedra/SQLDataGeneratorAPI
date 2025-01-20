using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SqlDataGenerator.Abstract;
using SqlDataGenerator.Endpoints;
using SqlDataGenerator.Logic;
using SQLDataGeneratorAPI.DataAccess.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Net.WebRequestMethods;
using SqlDataGeneratorAPI.Endpoints.Endpoints;

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

app.MapNameGenerationEndpoints();


app.Run();


