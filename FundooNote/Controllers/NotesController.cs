using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using CommonLayer.Model;
using MassTransit.Internals.GraphValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using RepoLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBusiness notesBusiness;

        //RADDIS:-
        private readonly IDistributedCache distributedCache;

        public NotesController(INotesBusiness notesBusiness, IDistributedCache distributedCache)
        {
            this.notesBusiness = notesBusiness;
            this.distributedCache = distributedCache;
        }


        // CREATE NOTE:-
        [Authorize]
        [HttpPost]
        [Route("CreateNotes")]
        public IActionResult CreateNote(NotesCreateModel model)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                scopedUserIdService.UserId = userId;

                var result = notesBusiness.CreateNotes(model);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Note Created Successful", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Note is not Created", data = result });
                }
            }
            else
            {
                return BadRequest("Invalid User");
            }
        }




        // GETTING NOTES LIST USING RADDIS CACHE :-
        [Authorize]
        [HttpGet]
        [Route("GetAllNotes")]
        public async Task<IActionResult> GetAllNotes()
        {
            var cacheKey = $"noteList_{User.FindFirst("UserId").Value}";
            var serializedNotesList = await distributedCache.GetStringAsync(cacheKey);
            List<NoteEntity> notesList;

            if (serializedNotesList != null)
            {
                notesList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNotesList);
            }
            else
            {
                var userIdClaim = User.FindFirst("UserId");
                if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
                {
                    var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                    scopedUserIdService.UserId = userId;

                    notesList = notesBusiness.GetAllNotes();
                    serializedNotesList = JsonConvert.SerializeObject(notesList);

                    await distributedCache.SetStringAsync(cacheKey, serializedNotesList,
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                            SlidingExpiration = TimeSpan.FromMinutes(2)
                        });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Invalid user ID claim" });
                }
            }

            if (notesList != null)
            {
                return Ok(new { success = true, message = "Notes getting successful.", data = notesList });
            }
            else
            {
                return NotFound(new { success = false, message = "Notes not found", data = notesList });
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
        public IActionResult UpdateNote(NoteUpdateModel model, long NoteID)
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
                if (result != null)
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
            //var userIdClaim = Convert.ToInt32( User.Claims.FirstOrDefault(e=>e.Type== "UserId").Value);
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





        //image upload :-
        [HttpPost]
        [Route("ImageUpload")]
        public async Task<IActionResult> AddImage(long id, IFormFile imageFile)
        {

            var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            Tuple<int, string> result = await notesBusiness.Image(id, userId, imageFile);
            if (result.Item1 == 1)
            {
                return Ok(new { success = true, messege = "Image Update  Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Image Update  Unucessfully", data = result });
            }
        }




    }
}