using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.Models;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly FundooDBContext context;
        private readonly INotesBusiness notesBusiness;
        private readonly IDistributedCache distributedCache;

        public NotesController(INotesBusiness notesBusiness, IDistributedCache distributedCache)
        {
            this.notesBusiness = notesBusiness;
            this.distributedCache = distributedCache;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateNote")]
        public ActionResult CreateNote(NotesModel notesModel)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                NoteEntity notesEntity = notesBusiness.CreateNote(UserId, notesModel);
                if (notesEntity != null)
                {
                    return Ok(new ResponseModel<NoteEntity> { IsSuccess = true, Message = "Notes added successfully", Data = notesEntity });
                }
                else
                {
                    return BadRequest(new ResponseModel<NoteEntity> { IsSuccess = false, Message = "Notes could not be created", Data = notesEntity });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("PinToggle")]
        public ActionResult PinToggle(int noteId)
        {
            NoteEntity note = notesBusiness.GetNoteById(noteId);
            if (note != null)
            {
                string message = "unpinned";
                if (note.IsPinned)
                {
                    message = "pinned";
                }
                notesBusiness.PinToggle(noteId);
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Note" + message });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Invalid Note Id" });
            }
        }

        [HttpPut("ArchiveToggle")]
        public ActionResult ArchiveToggle(int noteId)
        {
            try
            {
                NoteEntity note = notesBusiness.GetNoteById(noteId);
                if (note != null)
                {
                    string message = "unarchived";
                    if (note.IsArchived)
                    {
                        message = "archived";
                    }
                    notesBusiness.ArchiveToggle(noteId);
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Note" + message });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Invalid Note Id" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPut("TrashToggle")]
        public ActionResult TrashToggle(int noteId)
        {
            try
            {
                NoteEntity note = notesBusiness.GetNoteById(noteId);
                if (note != null)
                {
                    string message = "untrashed";
                    if (note.IsTrash)
                    {
                        message = "trashed";
                    }
                    notesBusiness.TrashToggle(noteId);
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Note" + message });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Invalid Note Id" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("SetBackGround")]
        public ActionResult SetBackground(int noteId, NoteBackgroundModel noteBackgroundModel)
        {
            if (!Enum.IsDefined(typeof(Colours), noteBackgroundModel.colour))
            {
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Invalid colour" });
            }

            try
            {
                var note = notesBusiness.GetNoteById(noteId);
                if (note != null)
                {
                    notesBusiness.SetBackground(noteId, noteBackgroundModel);
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Background changes applied" });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Error with the Background" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UploadImage")]
        public ActionResult Upload(int noteId, IFormFile imagePath)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var note = notesBusiness.GetNoteById(noteId);
                if (note != null)
                {
                    notesBusiness.UploadImage(noteId, userId, imagePath);
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Image added successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Error with adding Image" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("AddReminder")]
        public ActionResult AddReminder(int noteId, ReminderModel reminder)
        {
            try
            {
                var note = notesBusiness.GetNoteById(noteId);
                if (note != null)
                {
                    notesBusiness.AddReminder(noteId, reminder);
                    return Ok(new ResponseModel<DateTime> { IsSuccess = true, Message = "Reminder set successfully", Data = reminder.DateTime });
                }
                else
                {
                    return BadRequest(new ResponseModel<DateTime> { IsSuccess = false, Message = "Could not set Reminder", Data = reminder.DateTime });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpDelete("DeleteNote")]
        public ActionResult DeleteNote(int noteId, int userId)
        {
            try
            {
                bool noteIsDeleted = notesBusiness.DeleteNote(noteId, userId);
                if (noteIsDeleted)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Note deleted successfully", Data = true });
                }
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Note could not be deleted", Data = false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("GetUserNotesWithUserInfo")]
        public ActionResult GetUserNotesWithUserInfo()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var currentUserNotes = notesBusiness.GetUserNotesWithUserInfo(userId);
                if (currentUserNotes != null)
                {
                    return Ok(new ResponseModel<IList> { IsSuccess = true, Message = "Following are your notes along with user info", Data = currentUserNotes });
                }
                return BadRequest(new ResponseModel<IList> { IsSuccess = false, Message = "Notes with user info could not be fetched", Data = currentUserNotes });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("GetNotesWithLabels")]
        public ActionResult GetNotesWithLabels()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var currentUserNotesWithLabels = notesBusiness.GetNotesWithLabels(userId);
                if(currentUserNotesWithLabels != null)
                {
                    return Ok(new ResponseModel<IList> { IsSuccess= true, Message ="Following are your notes along with Labels Info", Data = currentUserNotesWithLabels });
                }
                return BadRequest(new ResponseModel<IList> { IsSuccess = false, Message = "Notes along with Labels info could not be fetched", Data = currentUserNotesWithLabels });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("GetNotesWithCollaboratorsAndLabels")]
        public ActionResult GetNotesWithCollaboratorsAndLabels()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var currentUserNotesWithCollaboratorsAndLabels = notesBusiness.GetNotesWithLabels(userId);
                if (currentUserNotesWithCollaboratorsAndLabels != null)
                {
                    return Ok(new ResponseModel<IList> { IsSuccess = true, Message = "Following are your notes along with Labels and Collaborator Info", Data = currentUserNotesWithCollaboratorsAndLabels });
                }
                return BadRequest(new ResponseModel<IList> { IsSuccess = false, Message = "Notes Labels and Collaborator info could not be fetched", Data = currentUserNotesWithCollaboratorsAndLabels });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAll/{authorId}/{enableCache}")]
        public async Task<IActionResult> GetAll(int userId, int noteId)
        {

            string cacheKey = "NotesList ";
            string serializedNotesList;
            var NotesList = new List<NoteEntity>();
            //var redisNotesList = await distributedCache.GetAsync(cacheKey);
            byte[] redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNotesList);
            }
            else
            {
                NotesList = context.Notes.ToList();
                serializedNotesList = JsonConvert.SerializeObject(NotesList); redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(NotesList);
        }

        [HttpGet("NoteCountOfEachUser")]
        public ActionResult NoteCountOfEachUser()
        {
            try
            {
                var noteCountOfEachUser = notesBusiness.NoteCountOfEachUser();
                if (noteCountOfEachUser != null)
                {
                    return Ok(noteCountOfEachUser);
                }
                else
                {
                    return BadRequest(new {Message = "Error while fetching"});
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("FindNotesByTitleAndDescription")]
        public ActionResult FindNotesByTitleAndDescription(string title, string description)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            var matchingNotes = notesBusiness.FindNotesByTitleAndDescription(userId, title, description);

            if (matchingNotes.Count == 1)
            {
                var singleNote = matchingNotes;
                return Ok(singleNote);
            }
            else if (matchingNotes.Count > 1)
            {
                return Ok(matchingNotes);
            }
            else
            {
                return NotFound("No notes found.");
            }
        }

    }
}
