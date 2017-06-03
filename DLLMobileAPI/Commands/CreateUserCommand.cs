using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DLLMobileAPI.Commands
{
    public class CreateUserCommand
    {
        public string Name { get; set; }

        public long Cpf{ get; set; }

        public long CellPhoneNumber { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}