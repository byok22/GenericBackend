using GraphQL.Types;
using Shared.Dtos;

namespace Presentation.GraphQL.Types.Role
{
    public class RoleType : ObjectGraphType<RoleDto>
    {
        public RoleType()
        {
            Field(x => x.PKRole).Description("Role ID");
            Field(x => x.RoleName).Description("Role Name");
            Field(x => x.Available).Description("Is Available");
        }
    }
}
