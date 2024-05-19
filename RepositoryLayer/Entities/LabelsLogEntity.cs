using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entities
{
    public class LabelsLogEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [ForeignKey("NoteLabel")]
        public int LabelId { get; set; }

        [ForeignKey("LabelNoteId")]
        public int NoteId { get; set; }

        [ForeignKey("NoteUser")]
        public int UserId { get; set; }
    }
}
