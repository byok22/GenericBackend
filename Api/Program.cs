using Infrastructure.Config;
using Infrastructure.Persitence;
using Serilog;
using Microsoft.Data.SqlClient;
using Domain.DataBase;
using Domain.Repositories;
using Infrastructure.Repositories;
using Application.CustomerUseCases;
using DotNetEnv;
using GraphQL;
using GraphQL.Types;
using GraphiQl;
using System.Text.Json.Serialization;
using Presentation.GraphQL.Mutation;
using Presentation.GraphQL.Queries;
using Presentation.GraphQL.Schemas;

var builder = WebApplication.CreateBuilder(args);


// Configure logging with Serilog
Log.Logger = new LoggerConfiguration()
.MinimumLevel.Debug()  //minimum level of the log sirve para indicar el nivel minimo de los logs que se van a guardar en este caso es debug entonces se guardaran todos los logs ya que debug es el nivel mas bajo hay otros niveles como information, warning, error, fatal y none  
.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();     
builder.Logging.AddSerilog();

// Load environment variables from .env file
Env.Load();

//Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



// Obtener la cadena de conexión de la clase estática ConnectionStrings
var connectionString = ConnectionStrings.CustomerService;

// Registrar la cadena de conexión como un servicio singleton
builder.Services.AddSingleton(connectionString);

// Agregar la configuración del servicio para SQLDbConnect y IConnectionDB
// Registrar SQLDbConnect como la implementación de IAppConnectionDB
builder.Services.AddSingleton<SqlConnection>(provider => new SqlConnection(connectionString));

builder.Services.AddSingleton<IAppConnectionDB, SQLDbConnect>();
builder.Services.AddSingleton<ISQLDbConnect, SQLDbConnect>();
builder.Services.AddSingleton<IPostgresqlConnect, PostgresqlConnect>();


//Dependency Injection for repositories
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();

//Dependency Injection for UseCases
  // Customers Example
builder.Services.AddScoped<CreateCustomerUseCase>();
builder.Services.AddScoped<GetCustomerByIdUseCase>();
builder.Services.AddScoped<GetAllCustomersUseCase>();
builder.Services.AddScoped<UpdateCustomerUseCase>();
builder.Services.AddScoped<DeleteCustomerUseCase>();
builder.Services.AddScoped<GetCustomerByCustomerIDUseCase>();


//Dependency Injection for Services
  

// Configuración de CORS
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers() 
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });




// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar GraphiQL y GraphQL
// Registrar tipos de GraphQL


            // Add other types here

            //// Registrar esquemas de GraphQL

           
    builder.Services.AddTransient<CustomerMutation>();
    builder.Services.AddTransient<RootQuery>();
    builder.Services.AddTransient<RootMutation>();
    builder.Services.AddTransient<ISchema, RootSchema>();

    builder.Services.AddGraphQL(builder => builder
        .AddAutoSchema<ISchema>()
        .AddSystemTextJson()
    .AddGraphTypes()   
    );



var app = builder.Build();

// Configurar GraphiQL y GraphQL
app.UseGraphiQl("/graphql");
app.UseGraphQL<ISchema>();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("PolicyCors");

app.UseAuthorization();
app.MapControllers();


app.Run();
