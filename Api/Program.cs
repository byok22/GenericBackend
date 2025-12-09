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
using Microsoft.AspNetCore.Mvc;
using Shared.Functions;
using Domain.Services;
using Infrastructure.Services;
using Application.UseCases.AuthUseCases;
using Api.Shared.Filters;
using Microsoft.OpenApi.Models;
using Application.HealthUseCases;
using Shared.Middleware;
using Application.Services;
using Extensions;
using Presentation.GraphQL.Types.Customer;
using Presentation.GraphQL.Types;
using Application.AppScreenUseCases;
using Application.RoleUseCases;
using Application.UserUseCases;

var builder = WebApplication.CreateBuilder(args);



// Configure logging with Serilog
Log.Logger = new LoggerConfiguration()
.MinimumLevel.Debug()  //minimum level of the log sirve para indicar el nivel minimo de los logs que se van a guardar en este caso es debug entonces se guardaran todos los logs ya que debug es el nivel mas bajo hay otros niveles como information, warning, error, fatal y none  
.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
  .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // Oculta información detallada de Microsoft
 .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning) // Oculta detalles de .NET
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.AddSerilog();

// Load environment variables from .env file
Env.Load();

//Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



// Obtener la cadena de conexión de la clase estática ConnectionStrings
//var connectionString = ConnectionStrings.CustomerService;

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Encryption key (should be stored securely)
var encryptionKey = "TE_GenericApi"; // Replace with your actual key


// Get the encrypted connection string from configuration
var encryptedConnectionString = builder.Configuration.GetConnectionString("SQLSERVER");

var connectionString = "";

// Decrypt the connection string
try{
connectionString = Encryp.DecryptConnectionString(encryptedConnectionString, encryptionKey);
} 
catch{
    Log.Error("Error al desencriptar la cadena de conexión");    
}

// Registrar la cadena de conexión como un servicio singleton
builder.Services.AddSingleton(connectionString);

// Dependency Injection
builder.Services.AddRepositories();
builder.Services.AddUseCases();
builder.Services.AddServices();

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

//Auth Example
builder.Services.AddScoped<LoginUseCase>();


//Dependency Injection for Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILdapService, LdapService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<LoginExceptionFilter>(); // Register the exception filter

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Health 
builder.Services.AddTransient<HealthUseCase>();


  

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



builder.Services.AddOutputCache();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var apiName = "My API"; // Define the apiName variable

builder.Services.AddSwaggerGen(c =>
{
    // Add API information
    c.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1" });
 
    // Define the Bearer authorization scheme being used
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
 
    // Add the Bearer authorization scheme to SwaggerUI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configurar GraphiQL y GraphQL
// Registrar tipos de GraphQL

// Customer Types
builder.Services.AddTransient<CustomerType>();
builder.Services.AddTransient<CustomerInputType>();
builder.Services.AddTransient<CustomerQuery>();
builder.Services.AddTransient<CustomerMutation>();

// AppScreen Types
builder.Services.AddTransient<Presentation.GraphQL.Types.AppScreen.AppScreenType>();
builder.Services.AddTransient<Presentation.GraphQL.Types.AppScreen.AppScreenInputType>();
builder.Services.AddTransient<AppScreenQuery>();
builder.Services.AddTransient<AppScreenMutation>();

// Role Types
builder.Services.AddTransient<Presentation.GraphQL.Types.Role.RoleType>();
builder.Services.AddTransient<Presentation.GraphQL.Types.Role.RoleInputType>();
builder.Services.AddTransient<RoleQuery>();
builder.Services.AddTransient<RoleMutation>();

// User Types
builder.Services.AddTransient<Presentation.GraphQL.Types.User.UserType>();
builder.Services.AddTransient<Presentation.GraphQL.Types.User.UserInputType>();
builder.Services.AddTransient<UserQuery>();
builder.Services.AddTransient<UserMutation>();

// Generic Types
builder.Services.AddTransient<GenericResponseType>();

// Root Types
builder.Services.AddTransient<CustomerMutation>();
builder.Services.AddTransient<RootQuery>();
builder.Services.AddTransient<RootMutation>();
builder.Services.AddTransient<ISchema, RootSchema>();

builder.Services.AddGraphQL(builder => builder
    .AddAutoSchema<ISchema>()
    .AddSystemTextJson()
.AddGraphTypes()   
);


// Add JWT authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseCors("PolicyCors");


app.MapGet("/health", async (HealthUseCase healthUseCase) =>
{
    var result = await healthUseCase.Excecute();
    return Results.Ok(result);
});

// Endpoint to encrypt connection string
app.MapPost("/encrypt-connection-string", ([FromBody] EncryptRequest request) =>
{
    var encryptedString = Encryp.EncryptConnectionString(request.ConnectionString, encryptionKey);
    return Results.Ok(encryptedString);
});

app.MapPost("/dencrypt-connection-string", ([FromBody] EncryptRequest request) =>
{
    var encryptedString = Encryp.DecryptConnectionString(request.ConnectionString, encryptionKey);
    return Results.Ok(encryptedString);
});

// Configurar GraphiQL y GraphQL
app.UseGraphiQl("/graphql");
app.UseGraphQL<ISchema>();


app.UseSwagger();
app.UseSwaggerUI();

//Middlewares
app.UseRequestTimeout();

app.UseHttpsRedirection();
app.UseCors("PolicyCors");


app.UseAuthentication(); // New
app.UseAuthorization();
app.MapControllers();
app.UseOutputCache();

app.Run();




public class EncryptRequest
{
    public string ConnectionString { get; set; }
}
