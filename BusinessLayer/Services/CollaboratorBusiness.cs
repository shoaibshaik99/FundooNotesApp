using BusinessLayer.Interfaces;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CollaboratorBusiness: ICollaboratorBusiness
    {
        private readonly ICollaboratorRepo collaboratorRepo;
        public CollaboratorBusiness(ICollaboratorRepo collaboratorRepo)
        {
            this.collaboratorRepo = collaboratorRepo;
        }

        public bool AddCollaborator(string collaboratorEmail, int noteId, int userId)
        {
            return collaboratorRepo.AddCollaborator(collaboratorEmail, noteId, userId);
        }

        public bool RemoveCollaborator(string collaboratorEmail, int noteId, int userId)
        {
            return collaboratorRepo.RemoveCollaborator(collaboratorEmail,noteId, userId);
        }
    }
}
