using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace BusinessLayer.Services
{
    public class UsersBusiness : IUserBusiness
    {
        private readonly IUserRepo userRepo;

        public UsersBusiness(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserEntity UserRegistration(RegisterModel model)
        {
            return userRepo.UserRegistration(model);
        }

        public string Login(UserLoginModel userLoginModel)
        {
            return userRepo.Login(userLoginModel);
        }

        public bool IsRegisteredEmail(string email)
        {
            return userRepo.IsRegisteredEmail(email);
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            return userRepo.ForgotPassword(email);
        }

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel)
        {
            return userRepo.ResetPassword(email, resetPasswordModel);
        }

        public UserEntity GetUsrByEmail(string email)
        {
            return userRepo.GetUserByEmail(email);
        }

        public UserEntity GetUserById(int userId)
        {
            return userRepo.GetUserById(userId);
        }

        public bool UpdateUserProfile(UpdateUserModel updateUserModel)
        {
            return userRepo.UpdateUserProfile(updateUserModel);
        }

        public int CheckAndUpdateUser(UpdateUserModel updateUserModel, string userEmail)
        {
            return userRepo.CheckAndUpdateUser(updateUserModel, userEmail);
        }

    }
}
