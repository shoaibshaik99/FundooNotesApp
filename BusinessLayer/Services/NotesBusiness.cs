using BusinessLayer.Interfaces;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NotesBusiness:INotesBusiness
    {
        private readonly INotesRepo notesRepo;
        public NotesBusiness (INotesRepo notesRepo)
        {
            this.notesRepo = notesRepo;
        }

        public NoteEntity GetNoteById(int noteId)
        {
            return notesRepo.GetNoteById(noteId);
        }

        public NoteEntity CreateNote(int UserId, NotesModel notesModel)
        {
            return notesRepo.CreateNote(UserId, notesModel);
        }

        public bool PinToggle(int noteId)
        {
            return notesRepo.PinToggle(noteId);
        }

        public bool ArchiveToggle(int noteId)
        {
            return notesRepo.ArchiveToggle(noteId);
        }

        public bool TrashToggle(int noteId)
        {
            return notesRepo.TrashToggle(noteId);
        }

        public string SetBackground(int noteId, NoteBackgroundModel noteBackgroundModel)
        {
            return notesRepo.SetBackground(noteId, noteBackgroundModel);
        }

        public string AddImage(int noteId, int userId, string filePath)
        {
            return notesRepo.AddImage(noteId, userId, filePath);
        }

        public ImageUploadResult UploadImage(int noteId, int userId, IFormFile imagePath)
        {
            return notesRepo.UploadImage(noteId, userId, imagePath);
        }

        public DateTime AddReminder(int noteId, ReminderModel reminder)
        {
            return notesRepo.AddReminder(noteId, reminder);
        }

        public bool DeleteNote(int noteId, int userId)
        {
            return notesRepo.DeleteNote(noteId, userId);
        }

        public IList GetUserNotesWithUserInfo(int userId)
        {
            return notesRepo.GetUserNotesWithUserInfo(userId);
        }

        public IList GetNotesWithLabels(int userId)
        {
            return notesRepo.GetNotesWithLabels(userId);
        }

        public IList GetNotesWithCollaboratorsAndLabels(int userId)
        {
            return notesRepo.GetNotesWithCollaboratorsAndLabels(userId);
        }

        public IList NoteCountOfEachUser()
        {
            return notesRepo.NoteCountOfEachUser();
        }

        public IList FindNotesByTitleAndDescription(int userId, string title, string description)
        {
            return notesRepo.FindNotesByTitleAndDescription(userId, title, description);
        }
    }
}
