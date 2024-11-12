using GraphQL.Types;
using Shared.Dtos;

namespace Presentation.GraphQL.Types.Customer
{
    public class CustomerType: ObjectGraphType<CustomerDto>
    {
        public CustomerType()
        {
            Field(x => x.CustomerID);
            Field(x => x.CustomerName);
            Field(x => x.Division);
            Field(x => x.BuildingID);         
            Field(x => x.Available);            
           
        }
        
    }
}