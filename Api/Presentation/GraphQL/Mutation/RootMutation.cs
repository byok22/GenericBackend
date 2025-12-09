using GraphQL.Types;

namespace   Presentation.GraphQL.Mutation
{
    public class RootMutation: ObjectGraphType
    {
        public RootMutation()
        {
            // Customer Mutations
            Field<CustomerMutation>("customerMutation")
                .Description("Mutations For Customer")
                .Resolve(context => new { });

            // AppScreen Mutations
            Field<AppScreenMutation>("appScreenMutation")
                .Description("Mutations For AppScreen")
                .Resolve(context => new { });

            // Role Mutations
            Field<RoleMutation>("roleMutation")
                .Description("Mutations For Role")
                .Resolve(context => new { });

            // User Mutations
            Field<UserMutation>("userMutation")
                .Description("Mutations For User")
                .Resolve(context => new { });
        }
        
    }
}