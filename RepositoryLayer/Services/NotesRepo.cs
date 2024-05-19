using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RepositoryLayer.Services
{
    public class NotesRepo : INotesRepo
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public NotesRepo(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public NoteEntity CreateNote(int userId, NotesModel notesModel)
        {
            if (!IsExistingUser(userId))
            {
                return null;
            }
            NoteEntity notesEntity = new NoteEntity();
            notesEntity.UserId = userId;
            notesEntity.Title = notesModel.Title;
            notesEntity.Description = notesModel.Description;
            notesEntity.CreatedAt = DateTime.Now;
            notesEntity.UpdatedAt = DateTime.Now;

            context.Notes.Add(notesEntity);
            context.SaveChanges();

            return notesEntity;
        }

        private bool IsExistingUser(int userId)
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null) { return true; };
            return false;
        }

        public NoteEntity GetNoteById(int noteId)
        {
            return context.Notes.FirstOrDefault(n => (n.NoteId == noteId));
        }

        public bool PinToggle(int noteId)
        {
            var note = GetNoteById(noteId);
            if (note != null)
            {
                note.IsPinned = !note.IsPinned;
            }
            context.SaveChanges();
            return note.IsPinned;
        }

        public bool ArchiveToggle(int noteId)
        {
            var note = GetNoteById(noteId);
            if (note != null)
            {
                if (note.IsPinned == true)
                {
                    note.IsPinned = false;
                }
                note.IsArchived = !note.IsArchived;
            }
            context.SaveChanges();
            return note.IsArchived;
        }

        public bool TrashToggle(int noteId)
        {
            var note = GetNoteById(noteId);
            if (note != null)
            {
                if (note.IsPinned == true)
                {
                    note.IsPinned = false;
                }
                if (note.IsArchived == true)
                {
                    note.IsArchived = false;
                }
                note.IsTrash = !note.IsTrash;
            }
            return note.IsTrash;
        }

        public string SetBackground(int noteId, NoteBackgroundModel noteBackgroundModel)
        {

            NoteEntity note = GetNoteById(noteId);
            if (note != null)
            {
                note.Colour = noteBackgroundModel.colour;
                return note.Colour;
            }
            else
            {
                return null;
            }

        }

        //First approach @31:22
        public string AddImage(int noteId, int userId, string filePath)
        {
            try
            {
                var filterUser = context.Notes.Where(e => e.UserId == userId);
                if (filterUser != null)
                {
                    var findNotes = filterUser.FirstOrDefault(e => e.NoteId == noteId);
                    if (findNotes != null)
                    {
                        Account account = new Account("dndg50tdt", "438242186751938", "FPjHEMQR6ggoVnLOeiCcYQv7WkE");
                        Cloudinary cloudinary = new Cloudinary(account);
                        ImageUploadParams uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(filePath),
                            PublicId = findNotes.Title
                        };
                        ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

                        findNotes.Image = uploadResult.Url.ToString();
                        findNotes.UpdatedAt = DateTime.Now;                        
                        context.SaveChanges();
                        return "Upload Successfull";
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ImageUploadResult UploadImage(int noteId, int userId, IFormFile imagePath)
        {
            try
            {
                var user = context.Notes.Where(e => e.UserId == userId);
                if (user != null)
                {
                    var note = user.FirstOrDefault(e => e.NoteId == noteId);
                    if (note != null)
                    {
                        Account account = new Account(configuration["Cloudinary:CloudName"], configuration["Cloudinary:ApiSecret"], configuration["Cloudinary:APIKey"]);
                        Cloudinary cloud = new Cloudinary(account);
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(imagePath.FileName, imagePath.OpenReadStream())//,
                            //PublicId = note.Title
                        };
                        var uploadImageRes = cloud.Upload(uploadParams);
                        note.Image = uploadImageRes.Url.ToString();
                        note.UpdatedAt = DateTime.Now;
                        context.SaveChanges();

                        if (uploadImageRes != null)
                            return uploadImageRes;
                        else
                            return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime AddReminder(int noteId, ReminderModel reminder)
        {
            var note = GetNoteById(noteId);
            if (note != null)
            {
                note.Reminder = reminder.DateTime;
                return note.Reminder;
            }
            else
                return DateTime.Now;
        }

        public bool DeleteNote(int noteId, int userId)
        {
            var note = context.Notes.FirstOrDefault(n => n.NoteId == noteId && n.UserId == userId);
            if (note == null)
            {
                return false;
            }
            context.Notes.Remove(note);
            context.SaveChanges();
            return true;
        }

        public IList GetUserNotesWithUserInfo(int userId)
        {
            //var user = fundooDBContext.Users.FirstOrDefault(u => u.UserId == userId);

            //var currentUser = (from u in fundooDBContext.Users
            //                   where u.UserId == userId
            //                   select u);

            //var allNotes = fundooDBContext.Notes.ToList();

            //SELECT N.NoteId, N.Title, N.Description, U.FirstName, U.LastName
            //FROM NoteEntity N
            //INNER JOIN UserEntity U ON N.UserId = U.UserId;


            var notesWithUserInfo = (from note in context.Notes
                                     join user in context.Users
                                     on note.UserId equals user.UserId
                                     select new
                                     {
                                         NoteId = note.NoteId,
                                         Title = note.Title,
                                         FirstName = user.FirstName,
                                         LastName = user.LastName
                                     }).ToList();

            //var notesWithUserInfo = fundooDBContext.Notes
            //                            .Join(fundooDBContext.Users,
            //                                note => note.UserId,
            //                                user => user.UserId,
            //                                (note, user) => new
            //                                {
            //                                    note.NoteId,
            //                                    note.Title,
            //                                    user.FirstName,
            //                                    user.LastName
            //                                })
            //                            .ToList();

            return notesWithUserInfo;
        }

        public IList GetNotesWithLabels(int userId)
        {
            var notesWithLabels = (from note in context.Notes
                                   join log in context.LabelsLogs
                                   on note.NoteId equals log.NoteId into noteLabels
                                   //where noteLabels != null
                                   from labelLog in noteLabels.DefaultIfEmpty()
                                   join label in context.Labels
                                   on labelLog.LabelId equals label.LabelId into noteLabelNames
                                   //where noteLabelNames != null
                                   from labelName in noteLabelNames.DefaultIfEmpty()
                                   select new
                                   {
                                       NoteId = note.NoteId,
                                       Title = note.Title,
                                       LabelName = labelName.LabelName
                                   }).ToList();

            return notesWithLabels;
        }

        public IList GetNotesWithCollaboratorsAndLabels(int userId)
        {
            var categorizedCollaborators = (from note in context.Notes
                                            join user in context.Users
                                            on note.UserId equals user.UserId
                                            join collaborator in context.Collaborators
                                            on note.NoteId equals collaborator.NoteId into noteCollaborators

                                            from collab in noteCollaborators.DefaultIfEmpty()
                                            join log in context.LabelsLogs
                                            on note.NoteId equals log.NoteId into noteLabels

                                            from labelLog in noteLabels.DefaultIfEmpty()
                                            select new
                                            {
                                                NoteId = note.NoteId,
                                                Title = note.Title,
                                                FirstName = user.FirstName,
                                                LastName = user.LastName,
                                                CollaboratorEmail = collab.CollaboratorEmail,
                                                LabelId = labelLog.LabelId
                                            }).ToList();

            return categorizedCollaborators;
        }
    
        //Find the count of notes a user owns and show the count along with user name.
        public IList NoteCountOfEachUser()
        {
            //var userNotesCount = (from note in context.Notes
            //                     join user in context.Users
            //                     on note.UserId equals user.UserId
            //                     select new
            //                     {
            //                         user.UserId,
            //                         UserName = $"{user.FirstName} + {user.LastName}",
            //                         note.NoteId
            //                     }).ToList().GroupBy(userId => userId).ToList();

            var users = (from note in context.Notes.ToList()
                         group note by note.UserId into groupedNotes
                         select new
                         {
                             UserId = groupedNotes.Key,
                             UserName = from user in context.Users where user.UserId == groupedNotes.Key select user.FirstName + user.LastName,
                             count = groupedNotes.Count()
                         }).ToList();


            if (users != null)
            {
                return users;
            }
            return null;
        }

        //Find the notes on the basis of title and description, if its a single note show single data else if
        //more than one note is found, show the list of notes
        public IList FindNotesByTitleAndDescription(int userId, string title, string description)
        {
            var matchingNotes = context.Notes
                .Where(note => note.Title == title && note.Description == description && note.UserId == userId)
                .ToList();

            return matchingNotes;
        }


    }
}