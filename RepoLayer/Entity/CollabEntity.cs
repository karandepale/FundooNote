using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RepoLayer.Entity
{
    public class CollabEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollabID { get; set; }
        public string Email { get; set; }


        // FOREIGN KEY USERID FROM USERTABLE:-
        [ForeignKey("User")]
        public long UserID { get; set; }
        [JsonIgnore]
        public UserEntity User { get; set; }




        // FOREIGN KEY NOTEID FROM NOTES TABLE
        [ForeignKey("Note")]
        public long NoteID { get; set; }
        [JsonIgnore]
        public NoteEntity Note { get; set; }
    }
}
