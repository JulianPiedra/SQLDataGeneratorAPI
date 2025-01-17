using SqlDataGenerator.Abstract;
using SqlDataGenerator.Endpoints;
using SqlDataGenerator.Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IIdGeneration, IdGeneration>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseRouting();
app.MapIdGenerationEndpoints();



app.Run();

