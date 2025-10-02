using ChessProject.DAL.Repositories;
using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Register(User user)
        {
            if (_userRepository.ExistByUserName(user.UserName))
            {
                throw new Exception($"User with {user.UserName} already exists");
            }
            if (_userRepository.ExistByEmail(user.Email))
            {
                throw new Exception($"User with {user.Email} already exists");
            }

            user.Role = Role.User;

            user.Password = user.UserName.Substring(0, 3).ToLower() + "1234";
            user.Password = Argon2.Hash(user.Password);

            if (user.ELO == null)
            {
                user.ELO = 1200;
            }

            _userRepository.Add(user);
        }

        public User Login(string login, string password)
        {
            User? user = _userRepository.GetUserByLogin(login);

            if (user == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (!Argon2.Verify(user.Password, password))
            {
                throw new Exception("Wrong password");
            }
            return user;
        }
    }
}
