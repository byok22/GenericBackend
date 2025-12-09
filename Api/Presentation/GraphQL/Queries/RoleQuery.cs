using Application.RoleUseCases;
using GraphQL.Types;
using GraphQL;
using Presentation.GraphQL.Types.Role;

namespace Presentation.GraphQL.Queries
{
    public class RoleQuery : ObjectGraphType
    {
        public RoleQuery(
            GetAllRoleUseCase getAllRoles,
            GetRoleByIdUseCase getRoleById)
        {
            Field<ListGraphType<RoleType>>("AllRoles")
                .Description("Get all Roles")
                .Resolve(context =>
                {
                    var result = getAllRoles.Execute().Result;
                    return result;
                });

            Field<RoleType>("RoleById")
                .Description("Get Role by ID")
                .Arguments(new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id", Description = "Role ID" }
                ))
                .Resolve(context =>
                {
                    var id = context.GetArgument<int>("id");
                    var result = getRoleById.Execute(id).Result;
                    return result;
                });
        }
    }
}
