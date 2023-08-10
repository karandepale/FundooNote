using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
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
            if (result != null)
            {
                return this.Ok(new { success = true, message = "User Registration Successful", data = result });
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
            if (result != null)
            {
                return Ok(new { success = true, message = "User Login Successful", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "User Login Failed", data = result });

            }
        }




        [HttpGet]
        [Route("UserList")]
        public IActionResult GetAllResult()
        {
            var result = userBusiness.GetAllUser();
            if (result != null)
            {
                return Ok(new { success = true, message = "User List Getting Successful", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "User List Getting Failed", data = result });

            }
        }




        [HttpGet]
        [Route("GetUserByID")]
        public IActionResult GetUserByID(long UserID)
        {
            var result = userBusiness.GetUserByID(UserID);
            if (result != null)
            {
                return Ok(new { success = true, message = "User By ID Getting Successful", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "User By ID Getting Failed", data = result });

            }
        }



        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            var result = userBusiness.ForgotPassword(model);
            if (result != null)
            {
                return Ok(new { success = true, message = "Forgot Pass Email Send Successfully" });
            }
            else
            {
                return NotFound(new { success = false, message = "Forgot pass email not send..." });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ResetPass")]
        public IActionResult ResetPassword(string newPass, string confirmPass)
        {
            var email = User.FindFirst("Email").Value;
            var result = userBusiness.ResetPassword(email, newPass, confirmPass);
            if (result != null)
            {
                return Ok(new { success = true, message = "Password Changed Successfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, message = "Password not changed", data = result });
            }
        }



    }
}