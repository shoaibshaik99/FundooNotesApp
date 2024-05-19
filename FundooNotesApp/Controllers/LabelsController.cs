using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelsRepo labelsBusiness;
        public LabelsController(ILabelsRepo labelsBusiness)
        {
            this.labelsBusiness = labelsBusiness;
        }

        [Authorize]
        [HttpPost("AddLabel")]
        public ActionResult AddLabelToNote(int noteId, string labelName)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var label = labelsBusiness.AddLabelToNote(userId, noteId, labelName);
                if (label != null)
                {
                    return Ok(new ResponseModel<LabelsLogEntity> { IsSuccess = true, Message = "Label Successfully Added", Data = label });
                }
                return BadRequest(new ResponseModel<LabelsLogEntity> { IsSuccess = false, Message = "THe specified Note does note exist", Data = label });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut("RemoveLabel")]
        public ActionResult RemoveLabelFromNote(int noteId, string labelName)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                bool IsLabelRemovedFromNotes = labelsBusiness.RemoveLabelFromNote(userId, noteId, labelName);
                if (IsLabelRemovedFromNotes)
                {
                    return Ok(new ResponseModel<bool>
                    {
                        IsSuccess = true,
                        Message = "Label successfully removed form Notes." +
                        "\nThe Label name still exists & can be applied to other notes",
                        Data = true
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Label can't be removed." +
                        "\nEither the Note or the Label does not exist" +
                        "or the Label is not applied to respective Note",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut("RenameLabel")]
        public ActionResult RenameLabel(string currentLabelName, string newLabelName)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                int freshLabelId = labelsBusiness.RenameLabel(userId, currentLabelName, newLabelName);
                if (freshLabelId != 0)
                {
                    return Ok(new ResponseModel<int> { IsSuccess = true, Message = "Label renamed successfully", Data = freshLabelId });
                }
                return BadRequest(new ResponseModel<int> { IsSuccess = false, Message = "Cannot perform rename", Data = freshLabelId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
