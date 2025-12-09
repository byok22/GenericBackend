using Application.RoleUseCases;
using GraphQL;
using GraphQL.Types;
using Presentation.GraphQL.Types;
using Presentation.GraphQL.Types.Role;
using Shared.Dtos;

namespace Presentation.GraphQL.Mutation
{
    public class RoleMutation : ObjectGraphType
    {
        public RoleMutation(
            InsertRoleUseCase insertRole,
            UpdateRoleUseCase updateRole,
            DeleteRoleUseCase deleteRole)
        {
            Field<GenericResponseType>("CreateRole")
                .Description("Create a new Role")
                .Arguments(new QueryArguments(
                    new QueryArgument<RoleInputType> { Name = "role", Description = "Role data" }
                ))
                .Resolve(context =>
                {
                    var role = context.GetArgument<RoleDto>("role");
                    return insertRole.Execute(role).Result;
                });

            Field<GenericResponseType>("UpdateRole")
                .Description("Update an existing Role")
                .Arguments(new QueryArguments(
                    new QueryArgument<RoleInputType> { Name = "role", Description = "Role data" }
                ))
                .Resolve(context =>
                {
                    var role = context.GetArgument<RoleDto>("role");
                    return updateRole.Execute(role).Result;
                });

            Field<GenericResponseType>("DeleteRole")
                .Description("Delete a Role")
                .Arguments(new QueryArguments(
                    new QueryArgument<RoleInputType> { Name = "role", Description = "Role data" }
                ))
                .Resolve(context =>
                {
                    var role = context.GetArgument<RoleDto>("role");
                    return deleteRole.Execute(role).Result;
                });
        }
    }
}
