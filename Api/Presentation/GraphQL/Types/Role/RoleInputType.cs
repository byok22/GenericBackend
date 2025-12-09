using GraphQL.Types;

namespace Presentation.GraphQL.Types.Role
{
    public class RoleInputType : InputObjectGraphType
    {
        public RoleInputType()
        {
            Field<IntGraphType>("PKRole").Description("Role ID");
            Field<StringGraphType>("RoleName").Description("Role Name");
            Field<BooleanGraphType>("Available").Description("Is Available");
        }
    }
}
