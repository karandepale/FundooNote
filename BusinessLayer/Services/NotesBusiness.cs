using BusinessLayer.Interfaces;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NotesBusiness : INotesBusiness
    {
        private readonly INotesRepo notesRepo;
        public NotesBusiness(INotesRepo notesRepo)
        {
            this.notesRepo = notesRepo;
        }

        public NoteEntity CreateNotes(NotesCreateModel model, long UserID)
        {
            try
            {
                return notesRepo.CreateNotes(model, UserID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<NoteEntity> GetAllNotes()
        {
            try
            {
                return notesRepo.GetAllNotes();   
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public NoteEntity UpdateNote(NoteUpdateModel model, long NoteID)
        {
            try
            {
                return notesRepo.UpdateNote(model, NoteID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        public void DeleteANote(long NoteID)
        {
            try
            {
                notesRepo.DeleteANote(NoteID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public NoteEntity GetANote(long NoteID)
        {
            try
            {
                return notesRepo.GetANote(NoteID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


       


        public List<NoteEntity> SearchNoteByQuery(string myinput)
        {
            try
            {
                return notesRepo.SearchNoteByQuery(myinput);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




        public bool IsArchive(long NoteID)
        {
            try
            {
                return notesRepo.IsArchive(NoteID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




        public bool IsTrash(long NoteID)
        {
            try
            {
                return notesRepo.IsTrash(NoteID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




        public bool IsPin(long NoteID)
        {
            try
            {
                return notesRepo.IsPin(NoteID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


       public Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            try
            {
                return notesRepo.UploadImageAsync(file);    
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


    }
}
