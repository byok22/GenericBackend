using GraphQL.Types;
using Presentation.GraphQL.Mutation;
using Presentation.GraphQL.Queries;


namespace Presentation.GraphQL.Schemas
{
    public class RootSchema: Schema
    {
        public RootSchema(IServiceProvider serviceProvider): base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<RootQuery>();
            Mutation = serviceProvider.GetRequiredService<RootMutation>();

            
        }
    }
}