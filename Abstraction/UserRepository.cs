﻿using System.Text;
using WebApp_vSem4.DataBase;
using WebApp_vSem4.DTO;
using WebApp_vSem4.Models;
using XSystem.Security.Cryptography;


namespace WebApp_vSem4.Abstraction
{
    public class UserRepository : IUserRepository
    {
        public int AddUser(UserDTO userDTO)
        {
            using (var context = new UserContext())
            {
                if (context.Users.Any(x => x.Name == userDTO.Name))
                    throw new Exception("User is already exist");

                if (userDTO.Role == UserRoleDTO.Admin)
                    if (context.Users.Any(x => x.RoleId == RoleId.Admin))
                        throw new Exception("Admin is already exist");

                var entity = new User() { Name = userDTO.Name!, RoleId = (RoleId)userDTO.Role };

                entity.Salt = new byte[16];
                new Random().NextBytes(entity.Salt);
                var data = Encoding.UTF8.GetBytes(userDTO.Password!).Concat(entity.Salt).ToArray();

                entity.Password = new SHA512Managed().ComputeHash(data);

                context.Users.Add(entity);
                context.SaveChanges();

                return entity.Id;
            }
        }

        public RoleId CheckUser(LoginDTO loginDTO)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Name == loginDTO.Name);

                if (user == null)
                    throw new Exception("No user like this!");

                var data = Encoding.UTF8.GetBytes(loginDTO.Password!).Concat(user.Salt!).ToArray();
                var hash = new SHA512Managed().ComputeHash(data);

                if (hash.SequenceEqual(user.Password!))
                    return user.RoleId;

                throw new Exception("Wrong password!");
            }
        }
    }
}
