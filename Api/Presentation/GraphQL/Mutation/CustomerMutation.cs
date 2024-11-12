using Application.CustomerUseCases;
using GraphQL.Types;
using Presentation.GraphQL.Types;
using Presentation.GraphQL.Types.Customer;
using Shared.Dtos;
using GraphQL;



namespace Presentation.GraphQL.Mutation
{
    public class CustomerMutation: ObjectGraphType
    {
        public CustomerMutation(CreateCustomerUseCase createCustomerUseCase)
        {
            Field<GenericResponseType>("CreateCustomer")
               .Description("Add Customer")
                .Arguments(new QueryArguments(
                    new QueryArgument<CustomerInputType> {Name = "customer"}
                ))
                .Resolve(context =>
                {
                    var customer = context.GetArgument<CustomerDto>("customer");
                    return createCustomerUseCase.Execute(customer).Result;
                });


            
        }
    }  
}