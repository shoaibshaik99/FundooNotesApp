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

        public bool IsRegisteredEmail(string email);

        public ForgotPasswordModel ForgotPassword(string email);

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel);

        public UserEntity GetUsrByEmail(string email);

        public UserEntity GetUserById(int userId);

        public bool UpdateUserProfile(UpdateUserModel updateUserModel);

        public int CheckAndUpdateUser(UpdateUserModel updateUserModel, string userEmail);
    }
}
