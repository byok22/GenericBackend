using Api.Domain.Models.DataBase;
using Api.Infrastructure.Config;
using Api.Infrastructure.Persitence;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);


// Obtener la cadena de conexión de la clase estática ConnectionStrings
var connectionString = ConnectionStrings.TE_RelLabTestPortal;

// Registrar la cadena de conexión como un servicio singleton
builder.Services.AddSingleton(connectionString);

// Agregar la configuración del servicio para SQLDbConnect y IConnectionDB
builder.Services.AddSingleton<IConnectionDB<SqlConnection, SqlParameter>>(provider =>
{
    var conn = new SqlConnection(connectionString);
    return new SQLDbConnect(conn);
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
