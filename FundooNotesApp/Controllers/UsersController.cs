using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using GreenPipes.Caching;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using System.IdentityModel.Tokens.Jwt;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _usersBusiness;
        private readonly IBus _bus;
        private readonly ILogger<UsersController> _logger;
        

        public UsersController(IUserBusiness usersBusiness, IBus bus, ILogger<UsersController> logger, ILogger<UsersController> _logger)
        {
            _usersBusiness = usersBusiness;
            _bus = bus;
            _logger = logger;
        }

        /* requestURL: https://localhost:44342/api/Users/Register */
        [HttpPost]
        [Route("Register")]

        
        public IActionResult Register(RegisterModel model)
        {
            var response = _usersBusiness.UserRegistration(model);
            if(response != null)
            {
                return Ok(new ResponseModel<UserEntity> {IsSuccess = true, Message = "Process Successful", Data = response});
                //200 status code, Process Successful
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { IsSuccess = false, Message = "Process Failed!", Data = response });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginModel userLoginModel)
        {
            try
            {
                var result = _usersBusiness.Login(userLoginModel);
                if (result != null)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Login Successful", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Login Failed", Data = result });
                }
                throw new Exception("Throwing sample exception to test NLog");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                if (_usersBusiness.IsRegisteredEmail(email))
                {
                    Send send = new Send();
                    ForgotPasswordModel forgotPasswordModel = _usersBusiness.ForgotPassword(email);
                    send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);

                    Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                    var endPoint = await _bus.GetSendEndpoint(uri);

                    await endPoint.Send(forgotPasswordModel);
                    
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Mail sent successfully", Data = forgotPasswordModel.Token});
                }
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Email does not exist", Data = null});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                //Can it be made until user provides matching passwords?
                if (resetPasswordModel.NewPassword != resetPasswordModel.ConfirmPassword)
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Both New Password and Confirm Password Should Match", Data = false });
                }
                
                string email = User.FindFirst("Email").Value;
                //string Email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;



                if (_usersBusiness.ResetPassword(email, resetPasswordModel))
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Password Updated Successfully", Data = true });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Password not Updated Successfully", Data = false });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateUser")]
        public IActionResult UpdateUserProfile(UpdateUserModel updateUserModel)
        {
            try
            {
                var result = _usersBusiness.UpdateUserProfile(updateUserModel);

                if (result)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "User Updated Successfully", Data = true });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "User not Updated Successfully", Data = false });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [HttpPut]
        [Route("CheckAndUpdateUser")]
        public IActionResult CheckAndUpdateUser(UpdateUserModel updateUserModel, string userEmail)
        {
            try
            {
                int result = _usersBusiness.CheckAndUpdateUser(updateUserModel,userEmail);
                if (result == 0)
                {
                    return BadRequest(new { Message = "Error" });
                }
                else if (result == 1)
                {
                    return Ok(new { Message = "User details updated successfully" });
                }
                else
                {
                    return Ok(new { Message = "New user created with default password as the user with provided details didn't exist" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
