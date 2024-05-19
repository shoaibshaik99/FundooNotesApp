using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorRepo
    {
        public bool AddCollaborator(string collaboratorEmail, int noteId, int userId);

        public bool RemoveCollaborator(string collaboratorEmail, int noteId, int userId);
    }
}
