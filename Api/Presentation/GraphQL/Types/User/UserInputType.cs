using GraphQL.Types;

namespace Presentation.GraphQL.Types.User
{
    public class UserInputType : InputObjectGraphType
    {
        public UserInputType()
        {
            Field<IntGraphType>("Id").Description("User ID");
            Field<StringGraphType>("UserName").Description("User Name");
            Field<StringGraphType>("NTUser").Description("NT User");
            Field<StringGraphType>("EmployeeNumber").Description("Employee Number");
            Field<StringGraphType>("Email").Description("User Email");
            Field<StringGraphType>("Role").Description("User Role");
            Field<BooleanGraphType>("Available").Description("Is Available");
        }
    }
}
