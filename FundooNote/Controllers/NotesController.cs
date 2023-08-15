using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RepoLayer.Interfaces;
using System;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBusiness notesBusiness;
        public NotesController(INotesBusiness notesBusiness)
        {
            this.notesBusiness = notesBusiness;
        }


        // CREATE NOTE:-
        [HttpPost]
        [Route("CreateNotes")]
        public IActionResult CreateNote(NotesCreateModel model , long UserID)
        {
            var result = notesBusiness.CreateNotes(model, UserID);
            if(result != null)
            {
                return Ok(new { success = true, message = "Note Created Successful", data = result });
            }
            else
            {
                return BadRequest(new { success = false, message = "Note is not Created", data = result });
            }
        }



        // GET NOTES lIST:-
        [Authorize]
        [HttpGet]
        [Route("GetAllNotes")]
        public IActionResult GetNotes()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                scopedUserIdService.UserId = userId;

                var result = notesBusiness.GetAllNotes();
                if (result != null)
                {
                    return Ok(new { success = true, message = "Notes Getting Successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = false, message = "Note List Not Getting", data = result });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid user ID claim" });
            }
        }





        [HttpGet]
        [Route("GetNoteByID")]
        public IActionResult GetNoteByID(long NoteID)
        {
            var result = notesBusiness.GetANote(NoteID);
            if (result != null)
            {
                return Ok(new { success = true, message = "Getting Note by ID Succefull", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "Note By ID Getting failed", data = result });
            }
        }




        // UPDATE A NOTE:-
        [HttpPost]
        [Route("UpdateNote")]
        public IActionResult UpdateNote(NoteUpdateModel model , long NoteID)
        {
            var result = notesBusiness.UpdateNote(model, NoteID);
            if (result != null)
            {
                return Ok(new { success = true, message = "Note Updated Succefully", data = result });
            }
            else
            {
                return BadRequest(new { success = false, message = "Note Not Updated", data = result });
            }
        }

        [HttpDelete]
        [Route("DeleteANote")]
        public IActionResult DeleteANote(long NoteID)
        {
            try
            {
                notesBusiness.DeleteANote(NoteID);
                return Ok(new { success = true, message = "Note Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Note Deletion Failed", error = ex.Message });
            }
        }





        [HttpGet]
        [Route("SearchNoteByQuery")]
        public IActionResult SearchNotes(string myinput)
        {
            try
            {
                var result = notesBusiness.SearchNoteByQuery(myinput);
                if(result != null)
                {
                    return Ok(new { success = true, message = "Searched Data successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = true, message = "Not Found Search input", data = result });
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




        //TOGGLE IMPLEMENTATION FOR ARCHIVE:-
        [HttpPost]
        [Route("Archive")]
        public IActionResult Archive(long NoteId)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                scopedUserIdService.UserId = userId;

                var result = notesBusiness.IsArchive(NoteId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Note Archived Successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = false, message = "Note Not Archived", data = result });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid user ID claim" });
            }
        }





        //TOGGLE IMPLEMENTATION FOR TRASH:-
        [HttpPost]
        [Route("Trash")]
        public IActionResult Trash(long NoteId)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                scopedUserIdService.UserId = userId;

                var result = notesBusiness.IsTrash(NoteId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Note Trashed Successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = false, message = "Note Not Trashed", data = result });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid user ID claim" });
            }
        }







        //TOGGLE IMPLEMENTATION FOR PIN:-
        [HttpPost]
        [Route("Pin")]
        public IActionResult Pin(long NoteId)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                scopedUserIdService.UserId = userId;

                var result = notesBusiness.IsPin(NoteId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Note Pinned Successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = false, message = "Note Not Pinned", data = result });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid user ID claim" });
            }
        }





        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            var uploadResult = await notesBusiness.UploadImageAsync(file);

            if (uploadResult.Error != null)
            {
                return BadRequest(uploadResult.Error.Message);
            }

            // Here, you can save the Cloudinary URL or any relevant information to your entity.
            string imageUrl = uploadResult.SecureUrl.AbsoluteUri;

            return Ok(new { success = true, message = "Image uploaded successfully", imageUrl });
        }



    }
}
