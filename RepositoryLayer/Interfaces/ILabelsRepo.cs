using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelsRepo
    {
        public LabelsLogEntity AddLabelToNote(int userId, int noteId, string labelName);

        public bool RemoveLabelFromNote(int userId, int noteId, string labelName);

        public int RenameLabel(int userId, string currentLabelName, string newLabelName);
    }
}
