using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface INotesBusiness
    {
        public NoteEntity CreateNote(int UserId, NotesModel notesModel);

        public NoteEntity GetNoteById(int noteId);

        public bool PinToggle(int noteId);

        public bool ArchiveToggle(int noteId);

        public bool TrashToggle(int noteId);

        public string SetBackground(int noteId, NoteBackgroundModel noteBackgroundModel);

        public string AddImage(int noteId, int userId, string filePath);

        public ImageUploadResult UploadImage(int noteId, int userId, IFormFile imagePath);

        public DateTime AddReminder(int noteId, ReminderModel reminder);

        public bool DeleteNote(int noteId, int userId);

        public IList GetUserNotesWithUserInfo(int userId);

        public IList GetNotesWithLabels(int userId);

        public IList GetNotesWithCollaboratorsAndLabels(int userId);

        public IList NoteCountOfEachUser();

        public IList FindNotesByTitleAndDescription(int userId, string title, string description);
    }
}
