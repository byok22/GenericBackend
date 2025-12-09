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
        private readonly ILogger<LoginUseCase> _logger;
        public LoginUseCase(
            ILdapService ldapService, 
            IMapper mapper,
            ITokenService tokenService,
            ILogger<LoginUseCase> logger
            
            ) : base(ldapService, mapper)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LdapLoginResponseDto> Execute(LdapLoginRequestDto request)
        {
          //  try{
                 var response = await _ldapService.Authenticate(request);

                // Find in your database if the user exists and complete the response

                //////
              

                // Find in your database if the user exists and complete the response
              //  var user = await _getUserByNTUserUseCase.Execute(response.NTUser);
              User user = new User();
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

                var userDto = _mapper.Map<UserDto>(user);

                //////
                var token = await _tokenService.GenerateToken(user);

                return new LdapLoginResponseDto
                {
                    Token = token,
                    User = userDto,
                    IsAuthenticated = true,
                    Message = "User" + response.UserName + "authenticated successfully"           
                };  
        }                
    }
}