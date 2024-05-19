using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICollaboratorBusiness
    {
        public bool AddCollaborator(string collaboratorEmail, int noteId, int userId);

        public bool RemoveCollaborator(string collaboratorEmail, int noteId, int userId);
    }
}
