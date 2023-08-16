using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepoLayer.Entity
{
    public class CollabEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollabID { get; set; }
        public string Email { get; set; }

        [ForeignKey("User")]
        public long UserID { get; set; }
        public UserEntity User { get; set; }

        [ForeignKey("Note")]
        public long NoteID { get; set; }
        public NoteEntity Note { get; set; }
    }
}
