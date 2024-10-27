using System.Text;
using WebApp_vSem4.DataBase;
using WebApp_vSem4.DTO;
using XSystem.Security.Cryptography;

namespace WebApp_vSem4.Abstraction
{
    public class UserAuthenticationService(UserContext _context) : IUserAuthenticationService
    {
        public UserDTO Authenticate(LoginDTO login)
        {
            using (_context)
            {
                if (_context.Users.Any(x => x.Name == login.Name))
                {
                    var user = _context.Users.FirstOrDefault(x => x.Name == login.Name);

                    var passwordCompare = Encoding.UTF8.GetBytes(login.Password!).Concat(user!.Salt!).ToArray();
                    var hash = new SHA512Managed().ComputeHash(passwordCompare);

                    if (hash.SequenceEqual(user.Password!))
                    {
                        return new UserDTO { Name = login.Name, Password = login.Password, Role = (UserRoleDTO)user.RoleId };
                    }
                    else
                    {
                        throw new Exception("Wrong password");
                    }
                }
                throw new Exception("User not found!");
            }
        }
    }
}
