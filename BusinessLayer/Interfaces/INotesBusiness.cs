using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface INotesBusiness
    {
        public NoteEntity CreateNotes(NotesCreateModel model, long UserID);
        public List<NoteEntity> GetAllNotes();
        public NoteEntity UpdateNote(NoteUpdateModel model, long NoteID);
        void DeleteANote(long NoteID);
        public NoteEntity GetANote(long NoteID);
        public List<NoteEntity> SearchNoteByQuery(string myinput);
        public bool IsArchive(long NoteID);
        public bool IsTrash(long NoteID);
        public bool IsPin(long NoteID);
    }
}
