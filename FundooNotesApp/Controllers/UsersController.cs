using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness usersBusiness;

        public UsersController(IUserBusiness usersBusiness)
        {
            this.usersBusiness = usersBusiness;
        }

        [HttpPost("Register")] //[Route("Register")]
        /* requestURL: https://localhost:44342/api/Users/Register */
        public IActionResult Register(RegisterModel model)
        {
            var response = usersBusiness.UserRegistration(model);
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

        [HttpGet("Login")]
        public IActionResult Login(UserLoginModel userLoginModel)
        {
            var result = usersBusiness.Login(userLoginModel);
            if(result != null)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Login Successful", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Login Failed", Data = result });
            }
        }

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            var result = usersBusiness.ForgotPassword(email);
            if( result != null)
            {
                return Ok(new ResponseModel<ForgotPasswordModel> {IsSuccess = true, Message = "User Exits", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<ForgotPasswordModel> { IsSuccess = false, Message = "User Not Found!", Data = result });
            }
        }

        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var result = usersBusiness.ResetPassword(resetPasswordModel);
            if (result != null)
            {
                return Ok(new ResponseModel<ResetPasswordModel> { IsSuccess = true, Message = "Password Updated Successfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<ResetPasswordModel> { IsSuccess = false, Message = "Password Updated Successfully", Data = result });
            }
        }
    }
}