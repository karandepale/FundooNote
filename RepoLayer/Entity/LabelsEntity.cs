using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepoLayer.Entity
{
    public class LabelsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelID { get; set; }
        public string Title { get; set; }



        // Foreign key USERID from User table:-
        [ForeignKey("User")]
        public long UserID { get; set; }
        [JsonIgnore]
        public UserEntity User { get; set; }



        // Foreign Key NoteID form Notes Table:-
        [ForeignKey("Note")]
        public long NoteID { get; set; }
        [JsonIgnore]
        public NoteEntity Note { get; set; }












    }
}
