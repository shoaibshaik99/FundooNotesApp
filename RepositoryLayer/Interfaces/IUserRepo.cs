using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        //Save Users registration details into DB
        public UserEntity UserRegistration(RegisterModel model);

        public string Login(UserLoginModel userLoginModel);

        public bool CheckEmailExists(string email);

    }
}
