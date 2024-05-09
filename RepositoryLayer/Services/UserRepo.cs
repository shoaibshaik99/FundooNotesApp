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
            this._config = config;
        }

        public UserEntity UserRegistration(RegisterModel model)
        {
            if (VerifyEmailExists(model.Email))
            {
                return null;
            }
            UserEntity userEntity = new UserEntity();

            userEntity.FirstName = model.FirstName;
            userEntity.lastName = model.LastName;
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

        //this function Convert to Decode your Password
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

        public bool VerifyEmailExists(string email)
        {
            var user = context.Users.FirstOrDefault(u => (u.Email == email));
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // To generate token
        private string GenerateToken(string email, int usrId)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("email",email),
                new Claim("usrId",usrId.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
                if (user != null)
                {
                    forgotPasswordModel.Email = email;
                    return forgotPasswordModel;
                }
                else
                {
                    return new ForgotPasswordModel();
                }
            }
            catch
            {
                return null;
            }


        }

        public ResetPasswordModel ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                UserEntity userEntity = GetUsrByEmail(resetPasswordModel.Email);
                if (userEntity != null && resetPasswordModel.Password == resetPasswordModel.ConfirmPassword)
                {
                    userEntity.Password = EncodePasswordToBase64(resetPasswordModel.Password);
                    userEntity.ChangedAt = DateTime.Now;
                    context.SaveChanges();
                }
                return resetPasswordModel;
            }
            catch
            {
                return null;
            }
        }

        public UserEntity GetUsrByEmail(string email)
        {
            try
            {
                UserEntity userEntity = context.Users.FirstOrDefault(u => u.Email == email);
                return userEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
