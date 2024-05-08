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

    }
}
