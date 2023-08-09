using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        
        private readonly IUserBusiness userBusiness;
        public UserController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
        }


        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(UserRegistrationModel model)
        {
            var result = userBusiness.UserRegistration(model);
            if(result != null)
            {
                return this.Ok(new { success = true  , message="User Registration Successful" , data=result});
            }
            else
            {
                return this.BadRequest(new { success = false, message = "User Registration Failed", data = result });
            }
        }



        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin(UserLoginModel model)
        {
            var result = userBusiness.UserLogin(model);
            if(result != null)
            {
                return Ok(new { success = true, message = "User Login Successful", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "User Login Failed", data = result });

            }
        }






    }
}
