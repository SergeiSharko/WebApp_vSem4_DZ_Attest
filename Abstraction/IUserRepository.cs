using WebApp_vSem4.DTO;
using WebApp_vSem4.Models;

namespace WebApp_vSem4.Abstraction
{
    public interface IUserRepository
    {
        int AddUser(UserDTO userDTO);
        RoleId CheckUser(LoginDTO loginDTO);        
    }
}
