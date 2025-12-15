using Application.UserUseCases;
using AutoMapper;
using Domain.Models;
using Domain.Services;
using Shared.Dtos;
using Shared.Dtos.Login;

namespace Application.UseCases.AuthUseCases
{
    public class LoginUseCase : AuthGenericUseCase
    {
        private readonly ITokenService _tokenService;
        private readonly GetUsersByWindowsIdUseCase _getUsersByWindowsIdUseCase;
        private readonly ILogger<LoginUseCase> _logger;
        public LoginUseCase(
            //Quitar ldapService si quiero entrar desde otro lado
            ILdapService ldapService,
            IMapper mapper,
            ITokenService tokenService,
            GetUsersByWindowsIdUseCase getUsersByWindowsIdUseCase,
            ILogger<LoginUseCase> logger

            ) : base(ldapService, mapper)
        {
            _getUsersByWindowsIdUseCase = getUsersByWindowsIdUseCase;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LdapLoginResponseDto> Execute(LdapLoginRequestDto request)
        {
          //  try{
                var response = new User();

                ///Alert for demo purposes only

                if(request.NtUser!= "admin" && request.Password != "admin")
                {

                    response = await _ldapService.Authenticate(request);
                    if (response == null )
                    {
                        return new LdapLoginResponseDto
                        {
                            IsAuthenticated = false,
                            Message = "LDAP authentication failed"
                        };
                    }
                    
                }



                

                 


                // Find in your database if the user exists and complete the response

                //////
              

                // Find in your database if the user exists and complete the response
                var user = await _getUsersByWindowsIdUseCase.Execute(request.NtUser, request.SiteId);
             // User user = new User();
                //If user is inactivated
                if(user != null && !user.Available){
                    return new LdapLoginResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "User is inactivated"
                    };
                }
                if(user == null || user.Id<=0){
                    return new LdapLoginResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "User not found"
                    };
                }

                var userDto = _mapper.Map<User>(user);

                //////
                var token = await _tokenService.GenerateToken(userDto);

                // 2. NUEVO: Generar Refresh Token (7 días)
                var refreshToken = await _tokenService.GenerateRefreshToken(userDto);

                return new LdapLoginResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    User = user,
                    IsAuthenticated = true,
                    Message = "User" + response.UserName + "authenticated successfully"           
                };  
        }                
    }
}