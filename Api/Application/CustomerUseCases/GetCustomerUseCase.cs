using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.CustomerUseCases
{
    public class GetCustomerUseCase
    {
        private readonly ICustomersRepository _repository;
        private readonly IMapper _mapper;
        public GetCustomerUseCase(ICustomersRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerDto?> Execute(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return _mapper.Map<CustomerDto>(entity);
        }
    }
}
