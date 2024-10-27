using Microsoft.AspNetCore.Mvc;
using WebApp_vSem4.Abstraction;
using WebApp_vSem4.DTO;
using WebApp_vSem4.Models;

namespace WebApp_vSem4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserRepository _userRepository) : ControllerBase
    {  
        [HttpPost("Add_User")]
        public ActionResult<int> AddUser(UserDTO userDTO)
        {
            try
            {
                return Ok($"User has been added successfully, his ID = {_userRepository.AddUser(userDTO)}");
            }
            catch (Exception ex)
            {
                return StatusCode(409, ex.Message);
            }
        }

        [HttpPost("Check_User")]
        public ActionResult<RoleId> CheckUser(LoginDTO loginDTO)
        {
            try
            {
                var roleId = _userRepository.CheckUser(loginDTO);
                return Ok($"The user exists, his status = {roleId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
