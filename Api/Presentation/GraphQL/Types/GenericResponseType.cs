using GraphQL.Types;
using Shared.Response;

namespace Presentation.GraphQL.Types
{
    public class GenericResponseType: ObjectGraphType<GenericResponse>
    {
        public GenericResponseType()
        {
            
            Field(x => x.Message).Description("Message Test");
            Field(x => x.IsSuccessful).Description("IsSuccessful of Test");
        }
    }
}