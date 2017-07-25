using DLLMobileAPI;
using DLLMobileAPI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

                    if (context.Users.Any(u => u.Cpf == createUserCommand.Cpf))
                    {
                        return Ok(new { message = "Usuario ja existe." });
                    }
                    else
                    {
                        newUser.Name = createUserCommand.Name;
                        newUser.UserName = createUserCommand.UserName;
                        newUser.Cpf = createUserCommand.Cpf;
                        newUser.CellPhoneNumber = createUserCommand.CellPhoneNumber;
                        newUser.Password = encryptedPassword;

                        context.Users.Add(newUser);
                        await context.SaveChangesAsync();
                        return Ok(new { message = "Usuário criado com sucesso." });
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { message = "Ocorreu um erro inesperado ao criar usuario." });
            }
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
                    ApplicationUser userEdited;
                    if (updateUserCommand.IsReset)
                    {
                        userEdited = context.Users.FirstOrDefault(u => u.UserName == updateUserCommand.UserName);
                    }
                    else
                    {
                        userEdited = context.Users.AsEnumerable().FirstOrDefault(u => u.UserName == updateUserCommand.UserName && crypt.BCrypt.Verify(updateUserCommand.Password, u.Password));
                    }

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
        public async Task<IHttpActionResult> GetRecoverCode([FromUri]string cpf)
        {
            long cellPhone;
            string verificationCode = new Random().Next(1000,9999).ToString();
            try
            {
                using (var context = new ApiContext())
                {
                    long lcpf = long.Parse(cpf);
                    cellPhone = context.Users.FirstOrDefault(u => u.Cpf == lcpf).CellPhoneNumber;
                    string apiKey = "8ff1c4c2";
                    string apiSecret = "24a67056d8efa4c4";
                    string to = cellPhone.ToString();
                    string from = "NexmoWorks";
                    string text = string.Format("Seu codigo de verificacao: {0}", verificationCode);
                    
                    using (WebClient client = new WebClient())
                    {

                        byte[] response = client.UploadValues("https://rest.nexmo.com/sms/json", new NameValueCollection()
                        {
                            { "api_key", apiKey },
                            { "api_secret", apiSecret },
                            { "to", to },
                            { "from", from },
                            { "text", text },
                        });

                        string result = System.Text.Encoding.UTF8.GetString(response);
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(new { message = "Erro ao gerar código de verificação" });
            }

            return Ok(new { code = verificationCode });
        }

    }
}