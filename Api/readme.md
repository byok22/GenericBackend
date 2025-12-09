# Project Setup Guide

## Version History

 **v1.0.3** - 2025-09-30
  - Added JWT Authentication middleware in `Program.cs`
  - Integrated `UseAuthentication` and `UseAuthorization` in pipeline
  - Now `HttpContext.User` is correctly populated with claims
  - Fixed issue where `CurrentUserService` always returned "Unknown"

- **v1.0.2** - 2024-12-02
    - Add LoginExceptionFilter (Custom Filter) for Exception Handling for Login LDAP
    - For Create a new Exception need to create a new class in the `Shared\Exceptions` folder
        - Use the `IExceptionFilter` interface to implement the exception
            - Example:
            ```csharp
            public class LoginExceptionFilter : IExceptionFilter
            {
                public void OnException(ExceptionContext context)
                {
                    // Handle the exception
                }
            }
            ```
        - Add the exception in the `program.cs` in the `ConfigureServices` method:
            ```csharp
            builder.Services.AddScoped<LoginExceptionFilter>(); // Register the exception filter
            ```
        - Add the Filter in the Controller:
            ```csharp
            [HttpPost("login")]
            [ServiceFilter(typeof(LoginExceptionFilter))] // Apply the exception filter to this action
            public async Task<ActionResult<LdapLoginResponseDto>> Login(LdapLoginRequestDto request)
            ```
    - The exception filter will handle the exception and return a custom response.
                Note: The filter exceptions help reduce the use of try-catch blocks in controllers and provide cleaner exception handling.
          

- **v1.0.1** - 2024-11-25
  - Added JWT authentication configuration
  - Updated TokenService to ensure key length is at least 16 characters
  - Added LDAP login functionalit
  - Add Examples for Authentication and Authorization in Customer Controller
  - Add Endpoint for Auth in Swagger

- **v1.0.0** - 
## General Extensions for VSCode

1. Install VSCode
2. Install the following extensions:
    - .Net Core Add Reference
    - .Net Extension Pack
    - .NET Install Tool
    - Activitus Bar
    - C#
    - C# Dev KIT
    - C# Extensions
    - Easy Snippet
    - IntelliCode for C# Dev Kit
    - Material Icon Theme
    - Nuget Gallery
    - vscode-icons

## Configure to Run .NET

1. Install .NET 8 SDK
2. Install .NET 8 Runtime

## Configure Project

1. Download the project from the repository
    -- git clone 
2. Open the project in VSCode
3. Open the terminal
4. Run the following commands:
    ```sh
    dotnet restore
    dotnet build
    dotnet run
    ```
5. Open the browser and go to the following URLs for documentation:
    - Swagger: [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html) --Change for the port you are using
    - GraphQL: [https://localhost:5001/graphql](https://localhost:5001/graphql) --Change for the port you are using

## Environment Configuration

1. Create a file named `.env` in the root of the project
2. Add the following variables:
    ```env
    ASPNETCORE_ENVIRONMENT=Development
    ConnectionStrings__DefaultConnection=Server=
    JWTSettings__SecretKey=
    ```

## Configure connection to the database

1. Go to the `appsettings.json` file
2. Add the connection string in the `ConnectionStrings` section - For Default, have the connection string for the SQLSERVER the STG Server

    ```json
    "ConnectionStrings": {
        "SQLSERVER": "/5ji7k0ql6UOxMx+7jI43zC43YAhGMveHVJ2kIOtryKZ5F/hvgg6UVrICSPXG56hbhkBFW4kighJyZTH8W8LnRuDgNiLbRtY11jgnCM4VZecNvuXG7pRMEr+DhudZFxVNYtbhzcznCZ3wd7wpnXIm2Yifj5kAfN3l+IgHoh2/xLKsTe+AW/doD+2y77YoRZMQ3vrUBTt1L4VoTqrgyesqEDYPygUkZ48b3ntPdqRn0jn6DoOk3Bga4VnIqSwSt+QRj2rAWz72wMdBHtdHLoNEg=="
    }
    ```

3. To encrypt the connection string, run the application and go to the Swagger and use the endpoint `/encrypt-connection-string`

4. If change the connection string need to restart the application






## For Publish

1. Run the following command:
    ```sh
    dotnet publish -c Release -r win-x64 --self-contained
    ```

## Application Architecture

The application uses a clean architecture with the following layers:

- **Application**: Contains the application logic
- **Domain**: Contains the business logic
- **Infrastructure**: Contains the data access logic
- **Presentation**: Contains the presentation logic
- **Shared**: Contains the shared logic

## Using AutoMapper

- Go to `Api\Shared\AutoMap\AutoMapperForApp.cs`
- The GraphQL types are in the folder `Presentation\GraphQL`
- The Controllers are in the folder `Presentation\Api`

## To Generate a New Endpoint

1. Create the Model in the Domain Layer on `Domain\Models`
2. Create the DTO in the Shared Layer on `Shared\Dtos`
3. Add the mapping in the `AutoMapperForApp.cs`
4. Create the Repository Interface on `Domain\Repositories`
5. Implement the Repository Interface on `Infrastructure\Repositories`
6. Create the UseCase in the Application Layer on `Application\UseCases`
7. Create the Controller in the Presentation Layer on `Presentation\Api`

## For GraphQL

1. Create the type in `Presentation\GraphQL\Types`
2. Create the Query in `Presentation\GraphQL\Queries`
3. Create the Mutation in `Presentation\GraphQL\Mutations`


### Dependency Injection  Examples:
1. Add the Dependency Injection in the `program.cs`

    ```csharp
    builder.Services.AddTransient<IYourRepository, YourRepository>()
    builder.Services.AddTransient<YourUseCase>()
    builder.Services.AddTransient<YourGraphQLType>()
    builder.Services.AddTransient<YourGraphQLQuery>()
    builder.Services.AddTransient<YourGraphQLMutation>()
    ```


### Create Git Ignore for .NET Cli
    
    ```sh
    dotnet new gitignore
    ```
### For Pull From Repository

1. Run the following command:
    ```sh
    git pull origin main
    ```

### For Push to Repository
1. Run the following command:
    ```sh
    git add .
    git commit -m "Message"
    git push origin main
    ```
### For Create New branch and change to it
1. Run the following command:
    ```sh
    git switch -c branch-name
    ```

### For Merge Branch to Main
```sh
git switch main
git merge branch-name
```
### Recommendation: 
1.  Befor merge create a copy of the branch to avoid losing the changes merge whit the feature branch
then merger whit the main branch.