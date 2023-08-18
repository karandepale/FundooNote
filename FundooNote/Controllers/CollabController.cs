using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBusiness collabBusiness;
        private readonly IDistributedCache distributedCache;
        public CollabController(ICollabBusiness collabBusiness , IDistributedCache distributedCache)
        {
            this.collabBusiness = collabBusiness;
            this.distributedCache = distributedCache;
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


       




        // COLLAB LIST USING RADDIS CACHE:-
        [HttpGet]
        [Route("GetAllCollabs")]
        public async Task<IActionResult> GetCollabs(long NoteID)
        {
            var key = "collabs";
            var cacheData = await distributedCache.GetStringAsync(key);
            List<CollabEntity> result;

            if(cacheData != null)
            {
                result = JsonConvert.DeserializeObject<List<CollabEntity>>(cacheData);
            }
            else
            {
                result = collabBusiness.GetAllCollabs(NoteID);
                cacheData = JsonConvert.SerializeObject(result);

                await distributedCache.SetStringAsync(key, cacheData , new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                });
            }
            if (result != null)
            {
                return Ok(new { success = true, message = "Collabs Getting Successfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "Collabs  Not Found", data = result });
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
