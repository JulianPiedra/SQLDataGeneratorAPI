using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SqlDataGenerator.Abstract.DependencyInjection;
using SqlDataGenerator.Endpoints;
using SqlDataGenerator.Logic.GenerationLogic;
using SQLDataGeneratorAPI.DataAccess.Models;
using SqlDataGeneratorAPI.Endpoints.Endpoints;

var builder = WebApplication.CreateBuilder(args);
string apiKey = builder.Configuration.GetValue<string>("ApiKey");
//CORS Configuration 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

//Database Configuration 
builder.Services.AddDbContext<SQLGeneratorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Swagger Configuration 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SQL Data Generator API",
        Version = "v1",
        Description = "An API for generating structured SQL test data",
        Contact = new OpenApiContact
        {
            Name = "Julian Piedra",
            Email = "julianpiedra15@gmail.com",
            Url = new Uri("https://julianpiedra.github.io/Portfolio/"),
        }
    });

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
//Authentication & Authorization 
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>(
        "ApiKey",
        opts => opts.ApiKey = builder.Configuration.GetValue<string>("ApiKey") ?? "No key provided"
    );


//Dependency Injection (Services) 
builder.Services.AddScoped<IIdGeneration, IdGeneration>();
builder.Services.AddScoped<INameGeneration, NameGeneration>();
builder.Services.AddScoped<ICountryGeneration, CountryGeneration>();
builder.Services.AddScoped<ICityGeneration, CityGeneration>();
builder.Services.AddScoped<IGenderGeneration, GenderGeneration>();
builder.Services.AddScoped<IEmailGeneration, EmailGeneration>();
builder.Services.AddScoped<IDateGeneration, DateGeneration>();
builder.Services.AddScoped<ITelephoneGeneration, TelephoneGeneration>();
builder.Services.AddScoped<INumberGeneration, NumberGeneration>();

var app = builder.Build();

//Middleware Configuration 
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "SQL Data Generator - API Docs";
    c.ConfigObject.AdditionalItems.Add("ApiKey", apiKey);
});

//Use the auth middleware after the Swagger middleware to display ui
app.UseAuthentication();
app.UseAuthorization();

//API Endpoints Mapping 
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
