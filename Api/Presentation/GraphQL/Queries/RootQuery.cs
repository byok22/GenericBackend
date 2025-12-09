using GraphQL.Types;

namespace  Presentation.GraphQL.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            // Customer Queries
            Field<CustomerQuery>("customerQuery")
                .Description("Customer queries")
                .Resolve(context => new { });

            // AppScreen Queries
            Field<AppScreenQuery>("appScreenQuery")
                .Description("AppScreen queries")
                .Resolve(context => new { });

            // Role Queries
            Field<RoleQuery>("roleQuery")
                .Description("Role queries")
                .Resolve(context => new { });

            // User Queries
            Field<UserQuery>("userQuery")
                .Description("User queries")
                .Resolve(context => new { });
        }
    }
}
