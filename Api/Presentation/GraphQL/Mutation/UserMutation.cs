using Application.UserUseCases;
using GraphQL;
using GraphQL.Types;
using Presentation.GraphQL.Types;
using Presentation.GraphQL.Types.User;
using Shared.Dtos;

namespace Presentation.GraphQL.Mutation
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(
            CreateUserUseCase createUser,
            UpdateUserUseCase updateUser,
            DeleteUserUseCase deleteUser)
        {
            Field<GenericResponseType>("CreateUser")
                .Description("Create a new User")
                .Arguments(new QueryArguments(
                    new QueryArgument<UserInputType> { Name = "user", Description = "User data" }
                ))
                .Resolve(context =>
                {
                    var user = context.GetArgument<UserDto>("user");
                    return createUser.Execute(user).Result;
                });

            Field<GenericResponseType>("UpdateUser")
                .Description("Update an existing User")
                .Arguments(new QueryArguments(
                    new QueryArgument<UserInputType> { Name = "user", Description = "User data" }
                ))
                .Resolve(context =>
                {
                    var user = context.GetArgument<UserDto>("user");
                    return updateUser.Execute(user).Result;
                });

            Field<GenericResponseType>("DeleteUser")
                .Description("Delete a User")
                .Arguments(new QueryArguments(
                    new QueryArgument<UserInputType> { Name = "user", Description = "User data" }
                ))
                .Resolve(context =>
                {
                    var user = context.GetArgument<UserDto>("user");
                    return deleteUser.Execute(user).Result;
                });
        }
    }
}
