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
    public class LabelsController : ControllerBase
    {
        //RADDIS CACHE:-
        private readonly IDistributedCache distributedCache;

        private readonly ILabelBusiness labelBusiness;
        public LabelsController(ILabelBusiness labelBusiness , IDistributedCache distributedCache)
        {
            this.labelBusiness = labelBusiness;
            this.distributedCache = distributedCache;
        }


        [HttpPost]
        [Route("CreateLabel")]
        public IActionResult CreateLabel(LabelCreateModel model , long NoteID)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                var scopedUserIdService = HttpContext.RequestServices.GetRequiredService<IScopedUserIdService>();
                scopedUserIdService.UserId = userId;

                var result = labelBusiness.CreateLabel(model, NoteID);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Label Created Successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = false, message = "Label Not Created", data = result });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid user ID claim" });
            }
        }






        // GET ALL LABELS USING RADDIS CACHE:-
        [HttpGet]
        [Route("GetAllLabels")]
        public async Task<IActionResult> GetLabels(long NoteID)
        {
            var myKey = "labelList";
            var serializeLabelList = await distributedCache.GetStringAsync(myKey);
            List<LabelsEntity> result;

            if(serializeLabelList != null)
            {
                result = JsonConvert.DeserializeObject<List<LabelsEntity>>(serializeLabelList);
            }
            else
            {
                result = labelBusiness.GetAllLabels(NoteID);
                serializeLabelList = JsonConvert.SerializeObject(result);

                await distributedCache.SetStringAsync(myKey, serializeLabelList, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                } );
            }

            if (result != null)
            {
                return Ok(new { success = true, message = "List Of Labels getting Successful.", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "List Of Labels Not getting.", data = result });
            }

        }






        [HttpPut]
        [Route("UpdateLabel")]
        public IActionResult UpdateLabel(LabelUpdateModel model, long LabelID)
        {
            var result = labelBusiness.UpdateLabel(model, LabelID);
            if (result != null)
            {
                return Ok(new { success = true, message = "Label Updated Succesfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "Label Not Updated", data = result });
            }
        }





        [HttpDelete]
        [Route("DeleteLabel")]
        public IActionResult DeleteLabel(long LabelID)
        {
            try
            {
                labelBusiness.DeleteLabel(LabelID);
                return Ok(new { success = true, message = "Label Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Label Deletion Failed", error = ex.Message });
            }
        }






    }
}
