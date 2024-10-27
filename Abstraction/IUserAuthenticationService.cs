using WebApp_vSem4.DTO;

namespace WebApp_vSem4.Abstraction
{
    public interface IUserAuthenticationService
    {
        UserDTO Authenticate(LoginDTO login);
    }
}
