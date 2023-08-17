using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RepoLayer.Interfaces;
using System;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBusiness collabBusiness;
        public CollabController(ICollabBusiness collabBusiness)
        {
            this.collabBusiness = collabBusiness;
        }

        //CREATE COLLAB :-
        [HttpPost]
        [Route("CreateCollab")]
        public IActionResult CreateCollab(CollabCreateModel model , long NoteID)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                scopedUserIdService.UserId = userId;

                var result = collabBusiness.CreateCollab(model, NoteID);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Collabs Created Successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = false, message = "Collab Not Created", data = result });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid user ID claim" });
            }
        }


        // GET ALL LIST OF COLLABS:-
        [HttpGet]
        [Route("GetAllCollabs")]
        public IActionResult GetAllCollabs()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                scopedUserIdService.UserId = userId;

                var result = collabBusiness.GetAllCollabs();
                if (result != null)
                {
                    return Ok(new { success = true, message = "Collabs Getting Successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = false, message = "Collabs List Not Getting", data = result });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid user ID claim" });
            }
        }



        // Delete a collabe:-
        [HttpDelete]
        [Route("DeleteCollab")]
        public IActionResult DeleteCollab(long CollabID)
        {
            try
            {
                collabBusiness.DeleteACollab(CollabID);
                return Ok(new { success = true, message = "Collab Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Collab Deletion Failed", error = ex.Message });
            }
        }



    }
}
