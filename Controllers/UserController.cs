using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InstagramScraperCLI.Service;
using InstagramScraperCLI.Model;


namespace InstagramScraperCLI.Model {
    
    public class RequestBody {
        public string AccessToken { get; set; }

        public string[] Users { get; set; }
    }
}

namespace InstagramScraperCLI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // POST api/user
        [HttpPost]
        public async Task<string> GetGroupOfUsersMediaAsync([FromBody] RequestBody Request)
        {
            try {
                Console.WriteLine($"Geting images for ${User}");
                Console.WriteLine($"Access Token for Dropbox account is ${Request.AccessToken}");
                var Service = new UserService(Request.AccessToken);
                foreach (var User in Request.Users) {
                    await Service.GetMediaByUser(User);
                }
                return $"Request for user {User} completed successfully";
            }

            catch (Exception e) {
                Console.WriteLine(e.Message);
                return $"Request for user {User} failed miserably";
            }
        }

         // GET api/user
        [HttpGet("{User}")]
        public async Task<string> GetUserMediaAsync([FromRoute] string User, [FromHeader] string AccessToken)
        {
            try {
                Console.WriteLine($"Geting images for ${User}");
                Console.WriteLine($"Access Token for Dropbox account is ${AccessToken}");
                var Service = new UserService(AccessToken);
                await Service.GetMediaByUser(User);
                return $"Request for user {User} completed successfully";
            }

            catch (Exception e) {
                Console.WriteLine(e.Message);
                return $"Request for user {User} failed miserably";
            }
        }
    }
}
