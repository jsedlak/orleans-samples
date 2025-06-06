using CachedSiloReads.SiloHost.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddFusionCache();
builder.Services.AddScoped<CachedWeatherService>();
builder.Services.AddScoped<WeatherService>();
builder.UseOrleans();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
