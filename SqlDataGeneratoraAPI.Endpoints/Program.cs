using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Endpoints;
using SqlDataGenerator.Logic.GenerationLogic;
using SQLDataGeneratorAPI.DataAccess.Models;
using SqlDataGeneratorAPI.Endpoints.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()  
               .AllowAnyMethod(); 
    });
});

builder.Services.AddDbContext<SQLGeneratorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X_API_KEY",
        Type = SecuritySchemeType.ApiKey,
        Description = "Unauthorized. API key is missing or invalid."
    });

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
builder.Services.AddScoped<ITelephoneGeneration, TelephoneGeneration>();
builder.Services.AddScoped<INumberGeneration, NumberGeneration>();

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
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapIdGenerationEndpoints();
app.MapCountryGenerationEndpoints();
app.MapNameGenerationEndpoints();
app.MapCityGenerationEndpoints();
app.MapGenderGenerationEndpoints();
app.MapEmailGenerationEndpoints();
app.MapDateGenerationEndpoints();
app.MapTelephoneGenerationEndpoints();
app.MapNumberGenerationEndpoints();

app.Run();
