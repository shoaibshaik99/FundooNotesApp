using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entities
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }

        public string CollaboratorEmail { get; set; }

        [ForeignKey("Note")]
        public int NoteId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
