using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InstagramScraperCLI.Service;

namespace InstagramScraperCLI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async ActionResult<IEnumerable<string>> GetUserMediaAsync([FromUrl] string User, [FromBody] string AccessToken)
        {
            try {
                var Service = new UserService();
                await Service.GetMediaByUser(User);
            }

            catch (Exception e) {
                
            }
        }
    }
}
