using Microsoft.EntityFrameworkCore.Internal;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollaboratorRepo: ICollaboratorRepo
    {
        private readonly FundooDBContext context;
        private readonly INotesRepo noteRepo;

        public CollaboratorRepo(FundooDBContext context, INotesRepo noteRepo)
        {
            this.context = context;
            this.noteRepo = noteRepo;
        }

        public bool AddCollaborator(string collaboratorEmail, int noteId, int userId)
        {
            var collaboratingUser = context.Users.FirstOrDefault(u => u.Email == collaboratorEmail);
            if (collaboratingUser == null)
            {
                return false;
            }

            var user = context.Users.FirstOrDefault(U => U.UserId == userId);
            if (user.Email == collaboratorEmail)
            {
                return false;
            }

            var note = context.Notes.FirstOrDefault(n=>n.NoteId == noteId && n.UserId == userId);
            if (note == null)
            {
                return false;
            }

            if (IsExistingCollaborator(collaboratorEmail))
            {
                return false;
            }

            CollaboratorEntity collaboratorLog = new CollaboratorEntity();
            collaboratorLog.CollaboratorEmail = collaboratorEmail;
            collaboratorLog.NoteId = noteId;
            collaboratorLog.UserId = userId;
            context.Collaborators.Add(collaboratorLog);

            NotesModel notesModel = new NotesModel { Title = note.Title, Description = note.Description};
            noteRepo.CreateNote(collaboratingUser.UserId, notesModel);

            context.SaveChanges();
            return true;
        }

        public bool RemoveCollaborator(string collaboratorEmail, int noteId, int userId)
        {
            //THe collaborator has to be a registered user
            var collaboratingUser = context.Users.FirstOrDefault(u => u.Email == collaboratorEmail);
            if (collaboratingUser == null)
            {
                return false;
            }
            //collaborator and the note sharing user can't be same.
            var user = context.Users.FirstOrDefault(U => U.UserId == userId);
            if (user.Email == collaboratorEmail)
            {
                return false;
            }
            //NOte must be existing.
            var note = context.Notes.FirstOrDefault(n => n.NoteId == noteId && n.UserId == userId);
            if (note == null)
            {
                return false;
            }

            CollaboratorEntity collaboratorLog = new CollaboratorEntity();
            collaboratorLog.CollaboratorEmail = collaboratorEmail;
            collaboratorLog.NoteId = noteId;
            collaboratorLog.UserId = userId;
            context.Collaborators.Remove(collaboratorLog);

            var CollaboratingUser = context.Users.FirstOrDefault(u=>u.Email == collaboratorEmail);
            noteRepo.DeleteNote(noteId, CollaboratingUser.UserId);


        NotesModel notesModel = new NotesModel { Title = note.Title, Description = note.Description };
            noteRepo.CreateNote(collaboratingUser.UserId, notesModel);

            context.SaveChanges();
            return true;

        }

        private bool IsExistingNote(int noteId)
        {
            var note = context.Notes.FirstOrDefault(n => n.NoteId == noteId);
            if (note != null)
            {
                return true;
            }
            return false;
        }

        private bool IsRegisteredUser(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        private bool IsExistingCollaborator(string email)
        {
            var collaborator = context.Collaborators.FirstOrDefault(c=>c.CollaboratorEmail== email);
            if (collaborator != null)
            {
                return true;
            }
            return false;
        }


    }
}
