using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DLLMobileAPI.Controllers
{
    [RoutePrefix("validations")]
    public class ValidationController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> ExistsCpf([FromUri]long cpf)
        {
            try
            {
                using (var context = new ApiContext())
                {
                    return Ok(new { exists = context.Users.Any(u => u.Cpf == cpf) });
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}