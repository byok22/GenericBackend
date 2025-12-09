using AutoMapper;
using Domain.Repositories;

namespace Application.AppScreenUseCases
{
    public class AppScreenGenericUseCase
    {
        public readonly IAppScreensRepository _repository;
        public readonly IMapper _mapper;
        public AppScreenGenericUseCase(IAppScreensRepository appScreensRepository, IMapper mapper)
        {
            _repository = appScreensRepository;
            _mapper = mapper;
        }
    }
}