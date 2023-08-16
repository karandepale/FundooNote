using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Interfaces
{
    public interface INotesRepo
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
        public Task<Tuple<int, string>> Image(long id, long usedId, IFormFile imageFile);
    }
}
