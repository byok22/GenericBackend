# Project Setup Guide

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
    - Prisma
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
    - Swagger: [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)
    - GraphQL: [https://localhost:5001/graphql](https://localhost:5001/graphql)

## Environment Configuration

1. Create a file named `.env` in the root of the project
2. Add the following variables:
    ```env
    ASPNETCORE_ENVIRONMENT=Development
    ConnectionStrings__DefaultConnection=Server=
    JWTSettings__SecretKey=
    ```

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
4. Add the Dependency Injection in the `program.cs`

### For Example:

```csharp
builder.Services.AddTransient<IYourRepository, YourRepository>();
builder.Services.AddTransient<YourUseCase>();
builder.Services.AddTransient<YourGraphQLType>();
builder.Services.AddTransient<YourGraphQLQuery>();
builder.Services.AddTransient<YourGraphQLMutation>();