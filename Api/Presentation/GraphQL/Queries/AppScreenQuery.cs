using Application.AppScreenUseCases;
using GraphQL.Types;
using GraphQL;
using Presentation.GraphQL.Types.AppScreen;
using Shared.Dtos;

namespace Presentation.GraphQL.Queries
{
    public class AppScreenQuery : ObjectGraphType
    {
        public AppScreenQuery(
            GetAllAppScreensUseCase getAllAppScreens,
            GetAppScreenByIdUseCase getAppScreenById,
            GetAppScreensUseCase getAppScreens)
        {
            Field<ListGraphType<AppScreenType>>("AllAppScreens")
                .Description("Get all AppScreens")
                .Resolve(context =>
                {
                    var result = getAllAppScreens.Execute().Result;
                    return result;
                });

            Field<ListGraphType<AppScreenType>>("AvailableAppScreens")
                .Description("Get all available AppScreens")
                .Resolve(context =>
                {
                    var result = getAllAppScreens.Execute().Result;
                    return result.Where(x => x.Available == true).ToList();
                });

            Field<ListGraphType<AppScreenType>>("UnavailableAppScreens")
                .Description("Get all unavailable AppScreens")
                .Resolve(context =>
                {
                    var result = getAllAppScreens.Execute().Result;
                    return result.Where(x => x.Available == false).ToList();
                });

            Field<AppScreenType>("AppScreenById")
                .Description("Get AppScreen by ID")
                .Arguments(new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id", Description = "AppScreen ID" }
                ))
                .Resolve(context =>
                {
                    var id = context.GetArgument<int>("id");
                    var result = getAppScreenById.Execute(id).Result;
                    return result;
                });

            Field<ListGraphType<AppScreenType>>("AppScreensByToken")
                .Description("Get AppScreens by current user token")
                .Resolve(context =>
                {
                    var result = getAppScreens.Execute().Result;
                    return result;
                });
        }
    }
}
