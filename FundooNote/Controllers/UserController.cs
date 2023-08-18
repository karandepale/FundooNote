using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepoLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBusiness userBusiness;
        //RADDIS:-
        private readonly IDistributedCache distributedCache;
        public UserController(IUserBusiness userBusiness , IDistributedCache distributedCache)
        {
            this.userBusiness = userBusiness;
            this.distributedCache = distributedCache;
        }



        // USER REGISTRATION API:-
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


        // USER LOGIN API
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


       

        // DISTRIBUTED CACHING RADDIS:-
        // GET USER'S LIST API:-
        [HttpGet]
        [Route("UserList")]
        public async Task<IActionResult> GetAllResult()
        {
            var cacheKey = "userList";
            var serializedUserList = await distributedCache.GetStringAsync(cacheKey);

            List<UserEntity> userList;

            if (serializedUserList != null)
            {
                userList = JsonConvert.DeserializeObject<List<UserEntity>>(serializedUserList);
            }
            else
            {
                userList = userBusiness.GetAllUser();

                serializedUserList = JsonConvert.SerializeObject(userList);
                await distributedCache.SetStringAsync(cacheKey, serializedUserList, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                });
            }

            if (userList != null)
            {
                return Ok(new { success = true, message = "User List Getting Successful", data = userList });
            }
            else
            {
                return NotFound(new { success = false, message = "User List Getting Failed" });
            }
        }



        // GET USER BY THEIR USERID:-
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