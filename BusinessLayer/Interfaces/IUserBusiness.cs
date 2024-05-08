using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        public UserEntity UserRegistration(RegisterModel model);

        public string Login(UserLoginModel userLoginModel);

        public bool CheckEmailExists(string email);
    }
}
