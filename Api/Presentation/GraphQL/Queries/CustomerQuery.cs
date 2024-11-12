
using Application.CustomerUseCases;
using GraphQL.Types;
using Presentation.GraphQL.Types.Customer;

namespace Presentation.GraphQL.Queries
{
    public class CustomerQuery: ObjectGraphType
    {
        public CustomerQuery(GetAllCustomersUseCase getAllCustomer)
        {
            
            Field<ListGraphType<CustomerType>>("Customers")
            .Description("Get all customers")          
           .ResolveAsync(async context => {
               
                var result = await getAllCustomer.Execute();
                return result;
            });

        }

        
    }
}