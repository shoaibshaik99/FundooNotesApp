using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBusiness: IUserBusiness
    {
        private readonly IUserRepo userRepo;

        public UserBusiness(IUserRepo userRepo)
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

        public bool CheckEmailExists(string email)
        {
            return userRepo.VerifyEmailExists(email);
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            return userRepo.ForgotPassword(email);
        }

        public ResetPasswordModel ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            return userRepo.ResetPassword(resetPasswordModel);
        }

        public UserEntity GetUsrByEmail(string email)
        {
            return userRepo.GetUsrByEmail(email);
        }

    }
}
