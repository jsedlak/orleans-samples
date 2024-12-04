using Microsoft.EntityFrameworkCore;
using OrleansSamples.Patterns.SatellitePattern.Domain.ServiceModel;
using OrleansSamples.Patterns.SatellitePattern.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();
builder.Services.AddDbContext<AccountDbContext>(options => options.UseInMemoryDatabase("accounts"));
builder.Services.AddScoped<IAccountStatusReadRepository, EfAccountStatusRepository>();
builder.Services.AddScoped<IAccountStatusWriteRepository, EfAccountStatusRepository>();
builder.UseOrleans();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
