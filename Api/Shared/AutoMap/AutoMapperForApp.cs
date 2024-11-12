

using AutoMapper;
using Domain.Models;
using Shared.Dtos;


namespace Shared.AutoMap
{
    public class AutoMapperForApp: Profile
    {
        public AutoMapperForApp()
        {

          //mapper for convert customer to customerDto
          CreateMap<Customer, CustomerDto>();
          //mapper for convert customerDto to customer
          CreateMap<CustomerDto, Customer>();
                 
            
        }

    
          /*Mapper.Initialize(cfg =>
            {
                cfg.RecognizePostfixes("Field");
                cfg.CreateMap<Source, Dest>();
            });
              CreateMap<MesDataDiag, MesDataDiagDto>()
               .ForMember(dest => dest.Department, opt => opt.UseValue("Development"));
            
            */
    }
}