using Application.UserUseCases;
using GraphQL.Types;
using GraphQL;
using Presentation.GraphQL.Types.User;

namespace Presentation.GraphQL.Queries
{
    public class UserQuery : ObjectGraphType
    {
        public UserQuery(
            GetAllUsersUseCase getAllUsers,
            GetUserByIdUseCase getUserById,
            GetUserByNTUserUseCase getUserByNTUser,
            GetUserByUserIDUseCase getUserByUserID,
            GetUserByUserNameUseCase getUserByUserName)
        {
            Field<ListGraphType<UserType>>("AllUsers")
                .Description("Get all Users")
                .Resolve(context =>
                {
                    var result = getAllUsers.Execute().Result;
                    return result;
                });

            Field<UserType>("UserById")
                .Description("Get User by ID")
                .Arguments(new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id", Description = "User ID" }
                ))
                .ResolveAsync(async context =>
                {
                    var id = context.GetArgument<int>("id");
                    var result = await getUserById.Execute(id);
                    return result;
                });

            Field<UserType>("UserByNTUser")
                .Description("Get User by NT User")
                .Arguments(new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "ntUser", Description = "NT User" }
                ))
                .Resolve(context =>
                {
                    var ntUser = context.GetArgument<string>("ntUser");
                    var result = getUserByNTUser.Execute(ntUser).Result;
                    return result;
                });

            Field<UserType>("UserByUserID")
                .Description("Get User by User ID")
                .Arguments(new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "userId", Description = "User ID" }
                ))
                .Resolve(context =>
                {
                    var userId = context.GetArgument<string>("userId");
                    var result = getUserByUserID.Execute(userId).Result;
                    return result;
                });

            Field<UserType>("UserByUserName")
                .Description("Get User by User Name")
                .Arguments(new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "userName", Description = "User Name" }
                ))
                .Resolve(context =>
                {
                    var userName = context.GetArgument<string>("userName");
                    var result = getUserByUserName.Execute(userName).Result;
                    return result;
                });
        }
    }
}
