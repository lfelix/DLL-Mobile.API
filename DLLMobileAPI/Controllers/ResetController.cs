using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using crypt = BCrypt.Net;

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


        [HttpPut]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ChangePassword(RecoverUserCommand recoverUserCommand)
        {
            string message;

            try
            {
                using (var context = new ApiContext())
                {
                    var userEdited = context.Users.FirstOrDefault(u => u.Cpf == recoverUserCommand.Cpf);
                    if (userEdited != null)
                    {
                        var salt = crypt.BCrypt.GenerateSalt();
                        string encryptedPassword = crypt.BCrypt.HashPassword(recoverUserCommand.NewPassword, salt);

                        userEdited.Password = encryptedPassword;

                        context.Users.AddOrUpdate(userEdited);
                        await context.SaveChangesAsync();
                        message = "Usuário alterado com sucesso.";

                        return Ok(new { success = true, message = message });
                    }
                    else
                    {
                        return BadRequest("Usuário não encontrado.");
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest("Ocorreu um erro ao alterar usuário.");
            }
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
                        context.LoginActivities.Attach(useractivity);
                        context.Entry(useractivity).State = System.Data.Entity.EntityState.Deleted;
                        await context.SaveChangesAsync();
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