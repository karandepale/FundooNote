using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Services
{
    public class NotesRepo : INotesRepo
    {
        private readonly FundooContext fundooContext;
        private readonly IScopedUserIdService scopedUserIdService;
        private readonly FileService fileService;
        private readonly Cloudinary cloudinary;

        public NotesRepo(FundooContext fundooContext, IScopedUserIdService _scopedUserIdService , IConfiguration configuration , FileService fileService , Cloudinary cloudinary)
        {
            this.fundooContext = fundooContext;
            scopedUserIdService = _scopedUserIdService;
            this.fileService = fileService;
            this.cloudinary = cloudinary; 
        }


        //CREATE NOTES METHOD IMPLEMENTATION:-
        public NoteEntity CreateNotes(NotesCreateModel model , long UserID)
        {
            try
            {
                NoteEntity noteEntity = new NoteEntity();

                noteEntity.UserID = UserID; 
                noteEntity.Title = model.Title;
                noteEntity.Description = model.Description;
                noteEntity.Reminder = model.Reminder;
                noteEntity.Background = model.Background;
                noteEntity.Image = model.Image;
                noteEntity.IsArchive  = model.IsArchive;
                noteEntity.IsPin = model.IsPin;
                noteEntity.IsTrash = model.IsTrash;

                fundooContext.Notes.Add(noteEntity);
                fundooContext.SaveChanges();

                if(noteEntity != null)
                {
                    return noteEntity;
                }
                else
                {
                    return null; 
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        // GET LIST OF NOTES:-
        public List<NoteEntity> GetAllNotes()
        {
            try
            {
                var userId = scopedUserIdService.UserId;

                var result = fundooContext.Notes.Where(note => note.UserID == userId).ToList();
                return result;
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
                var result = fundooContext.Notes.FirstOrDefault
                    (data=> data.NoteID == NoteID);
                if(result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }





        // UPDATE A NOTE:-
        public NoteEntity UpdateNote(NoteUpdateModel model , long NoteID)
        {
            try
            {
                var result = fundooContext.Notes.FirstOrDefault
                    ( data => data.NoteID == NoteID );

                if( result != null)
                {
                    result.Title = model.Title;
                    result.Description = model.Description;
                    result.Reminder = model.Reminder;
                    result.Background = model.Background;
                    result.Image = model.Image;
                    result.IsArchive = model.IsArchive;
                    result.IsPin = model.IsPin;
                    result.IsTrash = model.IsTrash;

                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        // DELETE A NOTE:-
        public void DeleteANote(long NoteID)
        {
            try
            {
                var notesToDelete = fundooContext.Notes.Where(note => note.UserID == NoteID);

                foreach (var note in notesToDelete)
                {
                    fundooContext.Notes.Remove(note);
                }

                fundooContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        // WORKSHOP TASK:- SERACH NOTES BASED ON STRING PARAMETER:-
        public List<NoteEntity> SearchNoteByQuery(string myinput)
        {
            try
            {
                var result = fundooContext.Notes.Where
                    (
                    data => data.Title.Contains(myinput) ||
                    data.Description.Contains(myinput) ||
                    data.Background.Contains(myinput) ||
                    data.Image.Contains(myinput)
                    ).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




        // ARCHIVE TOGGLE API:-
        public bool IsArchive(long NoteID)
        {
            var userId = scopedUserIdService.UserId;
            var note = fundooContext.Notes.FirstOrDefault(data => data.NoteID == NoteID && data.UserID == userId);

            if (note != null)
            {
                note.IsArchive = !note.IsArchive; 
                fundooContext.SaveChanges();
                return note.IsArchive; 
            }

            return false; 
        }


        //TRASH TOGGLE API:-
        public bool IsTrash(long NoteID)
        {
            var userId = scopedUserIdService.UserId;
            var result = fundooContext.Notes.FirstOrDefault
                (data => data.NoteID == NoteID && data.UserID == userId);

            if(result != null)
            {
                result.IsTrash = !result.IsTrash;
                fundooContext.SaveChanges();
                return result.IsTrash;
            }
            else
            {
                return false;
            }
        }






        // TOGGLE PIN API IMPLEMENTATION:-
        public bool IsPin(long NoteID)
        {
            var userId = scopedUserIdService.UserId;
            var result = fundooContext.Notes.FirstOrDefault
                (data => data.NoteID == NoteID && data.UserID == userId);

            if (result != null)
            {
                result.IsPin = !result.IsPin;
                fundooContext.SaveChanges();
                return result.IsPin;
            }
            else
            {
                return false;
            }
        }






        // IMAGE  UPLOAD ON CLOUDINARY:-
        public async Task<Tuple<int, string>> Image(long id, long usedId, IFormFile imageFile)
        {
            var result = fundooContext.Notes.FirstOrDefault(x => x.NoteID == id && x.UserID == usedId);
            if (result != null)
            {
                try
                {
                    var data = await fileService.SaveImage(imageFile);
                    if (data.Item1 == 0)
                    {
                        return new Tuple<int, string>(0, data.Item2);
                    }

                    var UploadImage = new ImageUploadParams
                    {
                        File = new CloudinaryDotNet.FileDescription(imageFile.FileName, imageFile.OpenReadStream())
                    };

                    ImageUploadResult uploadResult = await cloudinary.UploadAsync(UploadImage);
                    string imageUrl = uploadResult.SecureUrl.AbsoluteUri;
                    result.Image = imageUrl;

                    fundooContext.Notes.Update(result);
                    fundooContext.SaveChanges();

                    return new Tuple<int, string>(1, "Image Uploaded Successfully");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }




    }
}
