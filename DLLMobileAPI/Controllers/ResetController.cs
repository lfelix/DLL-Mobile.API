using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DLLMobileAPI.Controllers
{
    [RoutePrefix("reset")]
    public class ResetController : ApiController
    {

        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri]string cpf)
        {
            string username;
            try
            {
                using (var context = new ApiContext())
                {
                    long lcpf = long.Parse(cpf);
                    username = context.Users.FirstOrDefault(u => u.Cpf == lcpf).UserName;
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }

            return Ok(new { username = username });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromUri]string username)
        {
            try
            {
                using (var context = new ApiContext())
                {
                    var useractivity = context.LoginActivities.FirstOrDefault(l => l.User.UserName == username);
                    if(useractivity != null)
                    {
                        context.Entry(useractivity).State = System.Data.Entity.EntityState.Deleted;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }

            return Ok(new { username = username });
        }
    }
}