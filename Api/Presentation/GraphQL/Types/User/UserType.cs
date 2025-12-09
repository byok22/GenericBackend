using GraphQL.Types;
using Shared.Dtos;

namespace Presentation.GraphQL.Types.User
{
    public class UserType : ObjectGraphType<UserDto>
    {
        public UserType()
        {
            Field(x => x.Id).Description("User ID");
            Field(x => x.UserName).Description("User Name");
            Field(x => x.NTUser).Description("NT User");
            Field(x => x.EmployeeNumber).Description("Employee Number");
            Field(x => x.Email).Description("User Email");
            Field(x => x.Role).Description("User Role");
            Field(x => x.Available).Description("Is Available");
        }
    }
}
