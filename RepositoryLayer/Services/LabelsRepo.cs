using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelsRepo : ILabelsRepo
    {
        private readonly FundooDBContext context;

        public LabelsRepo(FundooDBContext context)
        {
            this.context = context;
        }

        public LabelsLogEntity AddLabelToNote(int userId, int noteId, string labelName)
        {
            if (IsExistingNote(noteId))
            {
                if (!LabelExists(labelName))
                {
                    LabelEntity label = CreateLabel(labelName);

                    LabelsLogEntity labelLog = RecordLog(label, noteId, userId);

                    return labelLog;
                }
                else if (LabelExists(labelName))
                {
                    LabelEntity label = GetLabelByName(labelName);

                    LabelsLogEntity labelLog = RecordLog(label, noteId, userId);

                    return labelLog;
                }
            }
            return null;

        }

        public bool RemoveLabelFromNote(int userId, int noteId, string labelName)
        {
            if (IsExistingNote(noteId))
            {
                if (!LabelExists(labelName))
                {
                    return false;
                }
                var label = GetLabelByName(labelName);
                LabelsLogEntity labelLog = context.LabelsLogs.FirstOrDefault(log => log.UserId == userId && log.NoteId == noteId && log.LabelId == label.LabelId);
                if (labelLog != null)
                {
                    context.LabelsLogs.Remove(labelLog);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        //public int RenameLabel(int userId, string currentLabelName, string newLabelName)
        //{
        //    int FreshLabelId = 0;

        //    // Fetch the label Id associated with the current label Name
        //    int currentLabelId = context.Labels
        //        .FirstOrDefault(label => label.LabelName == currentLabelName).LabelId;

        //    // Fetch label logs associated with the current label ID
        //    var labelLogs = context.LabelsLogs
        //        .Where(log => log.LabelId == currentLabelId);

        //    // Check if there are distinct user IDs among the fetched label logs
        //    var distinctUserIds = labelLogs.Select(log => log.UserId).Distinct().ToList();

            
        //    if (distinctUserIds.Count == 1)
        //    {
        //        // Only one unique user with the current label name
        //        // Update the label name directly in the labelsEntity table
        //        var labelToUpdate = context.Labels
        //            .FirstOrDefault(label => label.LabelName == currentLabelName);
        //        if (labelToUpdate != null)
        //        {
        //            labelToUpdate.LabelName = newLabelName;
        //            FreshLabelId = labelToUpdate.LabelId;
        //            context.SaveChanges();
        //        }
        //    }
        //    else
        //    {
        //        var 
        //        // Multiple users with the same label name
        //        // Create a new label entry with the new label name
        //        var newLabel = new LabelEntity { LabelName = newLabelName };
        //        //check if a label with new name?
        //        context.Labels.Add(newLabel);
        //        context.SaveChanges();
        //        FreshLabelId = newLabel.LabelId;

        //        var currentUserLabelLogsWithCurrentLabelName = labelLogs.Where(u => u.UserId == userId);
        //         // Update label IDs in label logs for the newly created label
        //        foreach (var log in currentUserLabelLogsWithCurrentLabelName)
        //        {
        //            log.LabelId = FreshLabelId;
        //        }
        //        context.SaveChanges();
        //    }
        //    return FreshLabelId;
        //}



        private bool LabelExists(string labelName)
        {
            var label = context.Labels.FirstOrDefault(l => l.LabelName == labelName);
            if (label != null)
            {
                return true;
            }
            return false;
        }

        private LabelEntity CreateLabel(string labelName)
        {
            //LabelEntity label = new LabelEntity() { LabelName = labelName };
            LabelEntity label = new LabelEntity();
            label.LabelName = labelName;
            context.Labels.Add(label);
            context.SaveChanges();
            return label;

        }

        private LabelEntity GetLabelByName(string labelName)
        {
            var label = context.Labels.FirstOrDefault(l => l.LabelName == labelName);
            return label;
        }

        private LabelsLogEntity RecordLog(LabelEntity label, int noteId, int userId)
        {
            LabelsLogEntity labelLog = new LabelsLogEntity();
            labelLog.LabelId = label.LabelId;
            labelLog.NoteId = noteId;
            labelLog.UserId = userId;
            context.LabelsLogs.Add(labelLog);
            context.SaveChanges();

            return labelLog;
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

        public int RenameLabel(int userId, string currentLabelName, string newLabelName)
        {
            throw new NotImplementedException();
        }
    }
}
