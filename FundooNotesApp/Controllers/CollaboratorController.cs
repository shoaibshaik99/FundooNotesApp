using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using System;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBusiness collaboratorBusiness;
        public CollaboratorController(ICollaboratorBusiness collaboratorBusiness)
        {
            this.collaboratorBusiness = collaboratorBusiness;
        }

        [Authorize]
        [HttpPost("AddCollaborator")]
        public ActionResult AddCollaborator(string collaboratorEmail, int noteId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                bool collaboratorIsAdded = collaboratorBusiness.AddCollaborator(collaboratorEmail, noteId, userId);
                if (collaboratorIsAdded)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = $"Successfully shared note with {collaboratorEmail}", Data = true});
                }
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = $"Collaboration failed", Data = false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpDelete("RemoveCollaborator")]
        public ActionResult RemoveCollaborator(string collaboratorEmail, int noteId)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            bool collaboratorIsRemoved = collaboratorBusiness.RemoveCollaborator(collaboratorEmail, noteId, userId);

            if(!collaboratorIsRemoved)
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = $"Successfully removed {collaboratorEmail} as a collaborator", Data = true });
            }
            return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = $"Error while removing {collaboratorEmail} as a collaborator", Data = true });

        }
    }
}
