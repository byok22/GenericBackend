using Domain.Models;
using Domain.Services;
using Shared.Dtos.Login;
using System.DirectoryServices;

namespace Infrastructure.Services
{
    public class LdapService : ILdapService
    {
        private readonly ILogger<LdapService> _logger;

        public LdapService(ILogger<LdapService> logger)
        {
            _logger = logger;
        }
        public async Task<User> Authenticate(LdapLoginRequestDto ldapLoginRequestDto)
        {
            // Authenticate user with LDAP             
            string LDAPPath = "LDAP://CORP.JABIL.ORG:636"; // LDAP path
            string user = ldapLoginRequestDto.NtUser;
            string password = ldapLoginRequestDto.Password;

        //    try
         //   {
                User userByNt;
                using (DirectoryEntry entry = new DirectoryEntry(LDAPPath, "jabil\\" + user, password, AuthenticationTypes.SecureSocketsLayer))
                {
                //    if (OperatingSystem.IsWindows())
                 //   {
                  //      try
                    //    {
                            object nativeObject = entry.NativeObject;
                            userByNt = await GetUserByNtUser(ldapLoginRequestDto.NtUser);
                     //   }
                    //    catch (DirectoryServicesCOMException ex) when (ex.ExtendedErrorMessage.Contains("Logon failure"))
                    //    {
                     //       throw new UnauthorizedAccessException("Invalid username or password.");
                    //    }
                     //   catch (DirectoryServicesCOMException ex) when (ex.ExtendedErrorMessage.Contains("The server is not operational"))
                      //  {
                    //        throw new Exception("LDAP server is not responding.");
                    //    }
                    //    catch (DirectoryServicesCOMException ex)
                    //    {
                    //        throw new Exception("An error occurred while communicating with the LDAP server.");
                     //   }
                  //  }
                  //  else
                  //  {
                  //      throw new PlatformNotSupportedException("LDAP authentication is only supported on Windows.");
                   // }
                }
                return userByNt;
            }
            // catch (UnauthorizedAccessException ex)
            // {
            //     _logger.LogError(ex, "Invalid username or password.");
            //     throw new UnauthorizedAccessException("Invalid username or password.");
            // }
            // catch (PlatformNotSupportedException ex)
            // {
            //     _logger.LogError(ex, "Platform not supported.");
            //     throw;
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, "Error authenticating user with LDAP");
            //     throw new Exception("Error authenticating user with LDAP", ex);
            // }
      //  }

        public async Task<User> GetUserByNtUser(string ntUser)
        {
            User ldapUser = new User();
                       
         //   try
         //   {

            DirectoryEntry dEntry = new DirectoryEntry("LDAP://CORP.JABIL.ORG:636");

            DirectorySearcher dSearch = new DirectorySearcher(dEntry);

            dSearch.Filter = "(&(objectClass=user)(samaccountname=" + ntUser + "))";

            SearchResult sr = dSearch.FindOne();

            if (sr != null){

                ldapUser.NTUser = sr.Properties["samaccountname"][0].ToString();

                ldapUser.Email = sr.Properties["mail"][0].ToString();

                ldapUser.UserName = sr.Properties["extensionattribute14"][0].ToString();                 
            }                                

          //  }
           // catch (Exception ex)
         //   {
          //    Console.WriteLine(ex.Message);
          //  }
            return ldapUser;     
        }
    }
}