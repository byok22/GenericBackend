

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
          
            //mapper for User to UserDto
        CreateMap<User, UserDto>();
        //mapper for UserDto to User
        CreateMap<UserDto, User>();

        //mapper for Role to RoleDto 
        CreateMap<Role, RoleDto>();
        //mapper for RoleDto to Role 
        CreateMap<RoleDto, Role>();

          CreateMap<AppScreenDto, AppScreen>();
        CreateMap<AppScreen, AppScreenDto>();

        CreateMap<AppScreenRoleDTO, AppScreenRole>();
        CreateMap<AppScreenRole, AppScreenRoleDTO>();
        

                 
            
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