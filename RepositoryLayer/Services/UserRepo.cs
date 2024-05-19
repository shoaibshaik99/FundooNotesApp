using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration _config;

        public UserRepo(FundooDBContext context, IConfiguration config)
        {
            this.context = context;
            _config = config;
        }

        public UserEntity UserRegistration(RegisterModel model)
        {
            if (IsRegisteredEmail(model.Email))
            {
                return null;
            }
            UserEntity userEntity = new UserEntity();

            userEntity.FirstName = model.FirstName;
            userEntity.LastName = model.LastName;
            userEntity.Email = model.Email;
            userEntity.Password = EncodePasswordToBase64(model.Password);
            userEntity.CreatedAt = DateTime.Now;
            userEntity.ChangedAt = DateTime.Now;

            context.Users.Add(userEntity);
            context.SaveChanges();

            return userEntity;
        }

        public string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(encData_byte);

            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public string DecodeFromBase64(string encodedData)
        {
            byte[] decodePassword = Convert.FromBase64String(encodedData);
            return Encoding.UTF8.GetString(decodePassword);
        }

        public string Login(UserLoginModel userLoginModel)
        {

            string encodedPassword = EncodePasswordToBase64(userLoginModel.Password);
            var result = context.Users.FirstOrDefault(u => u.Email == userLoginModel.Email && u.Password == EncodePasswordToBase64(userLoginModel.Password));
            //var result = context.Users.FirstOrDefault(u => u.Email == userLoginModel.Email && DecodeFromBase64(u.Password) == userLoginModel.Password);
            if (result != null)
            {
                var token = GenerateToken(result.Email, result.UserId);
                return token;
            }
            return null;
        }

        public bool IsRegisteredEmail(string email)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateToken(string email, int userId)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim("Email",email),
                new Claim("UserId",userId.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                //expires: DateTime.Now.AddMinutes(15),
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
            if (user != null)
            {
                forgotPasswordModel.Email = user.Email;
                forgotPasswordModel.UserId = user.UserId;
                forgotPasswordModel.Token = GenerateToken(user.Email, user.UserId);
                return forgotPasswordModel;
            }
            else
            {
                return null;
            }
        }

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel)
        {
            UserEntity userEntity = GetUserByEmail(email);
            if (userEntity != null)
            {
                userEntity.Password = EncodePasswordToBase64(resetPasswordModel.NewPassword);
                userEntity.ChangedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public UserEntity GetUserByEmail(string email)
        {
            try
            {
                UserEntity userEntity = context.Users.FirstOrDefault(u => u.Email == email);
                return userEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserEntity GetUserById(int userId)
        {
            try
            {
                var userInDb = context.Users.FirstOrDefault(u => u.UserId == userId);

                if (userInDb == null)
                {
                    return null;
                }

                return userInDb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUserProfile(UpdateUserModel updateUserModel)
        {
            try
            {
                var userInDb = context.Users.FirstOrDefault(u => u.Email == updateUserModel.Email);

                if (userInDb == null)
                {
                    return false;
                }

                userInDb.FirstName = updateUserModel.FirstName;
                userInDb.LastName = updateUserModel.LastName;
                userInDb.Email = updateUserModel.Email;
                userInDb.ChangedAt = DateTime.Now;

                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Check for data existence of user using any table column, if user exist update the data else insert the data.
        public int CheckAndUpdateUser(UpdateUserModel updateUserModel, string userEmail)
        {
            int actionCode = 0;
            var existingUser = context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (existingUser != null)
            {
                existingUser.FirstName = updateUserModel.FirstName;
                existingUser.LastName = updateUserModel.LastName;
                existingUser.Email = updateUserModel.Email;
                existingUser.ChangedAt = DateTime.Now;
                context.SaveChanges();
                actionCode = 1;
            }
            else
            {
                UserEntity userEntity = new UserEntity();

                userEntity.FirstName = updateUserModel.FirstName;
                userEntity.LastName = updateUserModel.LastName;
                userEntity.Email = updateUserModel.Email;
                userEntity.Password = EncodePasswordToBase64("DefaultPassword");
                userEntity.CreatedAt = DateTime.Now;
                userEntity.ChangedAt = DateTime.Now;
                context.Users.Add(userEntity);
                context.SaveChanges();
                actionCode = 2;
            }
            return actionCode;
        }

        //public bool DeleteUserProfile(int userId)
        //{

        //}


    }
}
