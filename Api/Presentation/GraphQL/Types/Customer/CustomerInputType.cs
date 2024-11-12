using GraphQL.Types;
using Shared.Dtos;

namespace Presentation.GraphQL.Types.Customer
{
    public class CustomerInputType: InputObjectGraphType<CustomerDto>
    {
        public CustomerInputType()
        {
            Name = "CustomerInput";
            Field(x => x.CustomerID).Description("The ID of the Customer.");
            Field(x => x.CustomerName).Description("The Name of the Customer.");
            Field(x => x.Division).Description("The Division of the Customer.");
            Field(x => x.BuildingID).Description("The Building ID of the Customer.");        
            Field(x => x.Available).Description("The availability status of the Customer.");            
        }
    }
   
}