using AutoMapper;
using Domain.Repositories;

using Infrastructure.Repositories;

using Application.CustomerUseCases;
//Use Cases
using Application.UserUseCases;

using Application.RoleUseCases;
using Application.AppScreenUseCases;
using Application.AppScreenRoleUseCases;
using Application.BuildingUseCases;
using Application.SiteUseCases;



namespace Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
                  
         
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IAppScreensRepository, AppScreensRepository>();

            services.AddScoped<IAppScreenRoleRepository, AppScreenRoleRepository>();

            // Sites & Buildings
            services.AddScoped<ISitesRepository, SitesRepository>();
            services.AddScoped<IBuildingsRepository, BuildingsRepository>();

            

            
                   
            // Add other repositories here

            return services;
        }

        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
          

       
            //Users
            services.AddScoped<CreateUserUseCase>();
            services.AddScoped<GetUserByIdUseCase>();
            services.AddScoped<GetAllUsersUseCase>();
            services.AddScoped<UpdateUserUseCase>();
            services.AddScoped<DeleteUserUseCase>();
            services.AddScoped<GetUserByUserIDUseCase>();
            services.AddScoped<GetUserByUserNameUseCase>();
            services.AddScoped<GetUserByNTUserUseCase>();
            services.AddScoped<GetUsersByWindowsIdUseCase>();
            
        
          
            //Roles
            services.AddScoped<InsertRoleUseCase>();
            services.AddScoped<DeleteRoleUseCase>();
            services.AddScoped<GetAllRoleUseCase>();
            services.AddScoped<GetRoleByIdUseCase>();
            services.AddScoped<UpdateRoleUseCase>();
          
          
           
            //App Screen
            services.AddScoped<CreateAppScreenUseCase>();
            services.AddScoped<DeleteAppScreenUseCase>();
            services.AddScoped<EditAppScreenUseCase>();
            services.AddScoped<GetAllAppScreensUseCase>();
            services.AddScoped<GetAppScreensUseCase>();
            services.AddScoped<GetAppScreenByIdUseCase>();
            services.AddScoped<GetAppScreenRolesByRoleUseCase>();
            services.AddScoped<SyncPermissionsForRoleUseCase>();

            // Building UseCases
            services.AddScoped<CreateBuildingUseCase>();
            services.AddScoped<EditBuildingUseCase>();
            services.AddScoped<GetAllBuildingsUseCase>();
            services.AddScoped<GetBuildingByIdUseCase>();
            services.AddScoped<DeleteBuildingUseCase>();

            // Site UseCases
            services.AddScoped<CreateSiteUseCase>();
            services.AddScoped<EditSiteUseCase>();
            services.AddScoped<GetAllSitesUseCase>();
            services.AddScoped<GetSiteByIdUseCase>();
            services.AddScoped<DeleteSiteUseCase>();
            
            


            


            // Add other use cases here

            return services;
        }

 public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // services.AddTransient<IMsgService, MsgService>();
            // services.AddTransient<IEmailService, EmailService>();
            // services.AddTransient<ILoggingService, LoggingService>();
            // Add other services here

            return services;
        }
        //AddGraphiQl
       
    }
}