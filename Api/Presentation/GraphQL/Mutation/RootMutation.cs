using GraphQL.Types;

namespace   Presentation.GraphQL.Mutation
{
    public class RootMutation: ObjectGraphType
    {
        public RootMutation()
        {
       
          
            Field<CustomerMutation>("customerMutation").Description("Mutations For Customer").Resolve(context=> new {});
             ///ADD OTHERS
          
        }
        
    }
}