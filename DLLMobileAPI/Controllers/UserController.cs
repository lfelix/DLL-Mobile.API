using DLLMobileAPI;
using DLLMobileAPI.Commands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        public async Task<IHttpActionResult> Post(CreateUserCommand createUserCommand)
        {
            string message;

            try
            {
                using (var context = new ApiContext())
                {
                    var salt = crypt.BCrypt.GenerateSalt();
                    string encryptedPassword = crypt.BCrypt.HashPassword(createUserCommand.Password, salt);
                    var newUser = new ApplicationUser();

                    newUser.Name = createUserCommand.Name;
                    newUser.UserName = createUserCommand.UserName;
                    newUser.Cpf = createUserCommand.Cpf;
                    newUser.CellPhoneNumber = createUserCommand.CellPhoneNumber;
                    newUser.Password = encryptedPassword;

                    context.Users.Add(newUser);
                    await context.SaveChangesAsync();
                    message = "Usuário criado com sucesso.";
                }
            }
            catch (Exception e)
            {
                message = "Ocorreu um erro ao criar usuário.";
            }

            return Ok(new { message = message });
        }
        
        [HttpPut]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Put(UpdateUserCommand updateUserCommand)
        {
            string message;

            try
            {
                using (var context = new ApiContext())
                {
                    var userEdited = context.Users.AsEnumerable().FirstOrDefault(u => u.UserName == updateUserCommand.UserName && crypt.BCrypt.Verify(updateUserCommand.Password, u.Password));

                    if (userEdited != null)
                    {
                        var salt = crypt.BCrypt.GenerateSalt();
                        string encryptedPassword = crypt.BCrypt.HashPassword(updateUserCommand.NewPassword, salt);

                        userEdited.UserName = updateUserCommand.UserName;
                        userEdited.Password = encryptedPassword;

                        context.Users.AddOrUpdate(userEdited);
                        await context.SaveChangesAsync();
                        message = "Usuário alterado com sucesso.";
                    }
                    else
                    {
                        message = "Usuário não encontrado.";
                    }
                }
            }
            catch (Exception e)
            {
                message = "Ocorreu um erro ao alterar usuário.";
            }

            return Ok(new { message = message });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> Phone([FromUri]string cpf)
        {
            long cellPhone;
            try
            {
                using (var context = new ApiContext())
                {
                    long lcpf = long.Parse(cpf);
                    cellPhone = context.Users.FirstOrDefault(u => u.Cpf == lcpf).CellPhoneNumber;
                }
            }
            catch (Exception e) 
            {
                return NotFound();
            }

            return Ok(new { cellPhone = "55"+cellPhone });
        }
    }
}