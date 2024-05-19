using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelsBusiness: ILabelsBusiness
    {
        private readonly ILabelsRepo labelsRepo;
        public LabelsBusiness(ILabelsRepo labelsRepo)
        {
            this.labelsRepo = labelsRepo;
        }

        public LabelsLogEntity AddLabelToNote(int userId, int noteId, string labelName)
        {
            return labelsRepo.AddLabelToNote(userId, noteId, labelName);
        }

        public bool RemoveLabelFromNote(int userId, int noteId, string labelName)
        {
            return labelsRepo.RemoveLabelFromNote(userId,noteId, labelName);
        }

        public int RenameLabel(int userId, string currentLabelName, string newLabelName)
        {
            return labelsRepo.RenameLabel(userId, currentLabelName, newLabelName);
        }
    }
}
