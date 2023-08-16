using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateCollab(CollabCreateModel model , long UserID , long NoteID)
        {
            var result = collabBusiness.CreateCollab(model , UserID , NoteID);
            if(result != null)
            {
                return Ok(new { success = true, message = "Collab Created Successful", data = result });
            }
            else
            {
                return BadRequest(new { success = false, message = "Collab is not Created", data = result });
            }
        }

    }
}
