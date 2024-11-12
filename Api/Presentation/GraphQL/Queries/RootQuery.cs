using GraphQL.Types;

namespace  Presentation.GraphQL.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            // Add your queries here       
            Field<CustomerQuery>("customerQuery").Resolve(context => new { });
        }
    }
}
