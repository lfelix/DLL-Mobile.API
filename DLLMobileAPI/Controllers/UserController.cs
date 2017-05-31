using DLLMobileAPI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DLLMobileAPI.Controllers
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            using (var context = new ApiContext())
            {
                return Ok(await context.Users.ToListAsync());
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(ApplicationUser user)
        {
            string message;

            try
            {
                using (var context = new ApiContext())
                {
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                    message = "User Created Successfully!";
                }
            }
            catch (Exception)
            {
                message = "Error on saving User.";
            }

            return Ok(message);
        }

    }
}