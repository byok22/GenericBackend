using Application.AppScreenUseCases;
using GraphQL;
using GraphQL.Types;
using Presentation.GraphQL.Types;
using Presentation.GraphQL.Types.AppScreen;
using Shared.Dtos;

namespace Presentation.GraphQL.Mutation
{
    public class AppScreenMutation : ObjectGraphType
    {
        public AppScreenMutation(
            CreateAppScreenUseCase createAppScreen,
            EditAppScreenUseCase editAppScreen,
            DeleteAppScreenUseCase deleteAppScreen)
        {
            Field<GenericResponseType>("CreateAppScreen")
                .Description("Create a new AppScreen")
                .Arguments(new QueryArguments(
                    new QueryArgument<AppScreenInputType> { Name = "appScreen", Description = "AppScreen data" }
                ))
                .Resolve(context =>
                {
                    var appScreen = context.GetArgument<AppScreenDto>("appScreen");
                    return createAppScreen.Execute(appScreen).Result;
                });

            Field<GenericResponseType>("EditAppScreen")
                .Description("Edit an existing AppScreen")
                .Arguments(new QueryArguments(
                    new QueryArgument<AppScreenInputType> { Name = "appScreen", Description = "AppScreen data" }
                ))
                .Resolve(context =>
                {
                    var appScreen = context.GetArgument<AppScreenDto>("appScreen");
                    return editAppScreen.Execute(appScreen).Result;
                });

            Field<GenericResponseType>("DeleteAppScreen")
                .Description("Delete an AppScreen")
                .Arguments(new QueryArguments(
                    new QueryArgument<AppScreenInputType> { Name = "appScreen", Description = "AppScreen data" }
                ))
                .Resolve(context =>
                {
                    var appScreen = context.GetArgument<AppScreenDto>("appScreen");
                    return deleteAppScreen.Execute(appScreen).Result;
                });
        }
    }
}
