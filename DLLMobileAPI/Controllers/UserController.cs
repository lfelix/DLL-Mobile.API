using DLLMobileAPI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using crypt = BCrypt.Net;

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
        [AllowAnonymous]
        public async Task<IHttpActionResult> Post(ApplicationUser user)
        {
            string message;

            try
            {
                using (var context = new ApiContext())
                {
                    var salt = crypt.BCrypt.GenerateSalt();
                    string encryptedPassword = crypt.BCrypt.HashPassword(user.Password, salt);
                    var newUser = new ApplicationUser();
                    newUser.Name = user.Name;
                    newUser.UserName = user.UserName;
                    newUser.Cpf = user.Cpf;
                    newUser.CellPhoneNumber = user.CellPhoneNumber;
                    newUser.Password = encryptedPassword;

                    context.Users.Add(newUser);
                    await context.SaveChangesAsync();
                    message = "User Created Successfully!";
                }
            }
            catch (Exception e)
            {
                message = "Error on saving User.";
            }

            return Ok(message);
        }

    }
}