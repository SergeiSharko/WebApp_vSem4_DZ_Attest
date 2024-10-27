namespace WebApp_vSem4.DTO
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public UserRoleDTO Role { get; set; }
    }
}
