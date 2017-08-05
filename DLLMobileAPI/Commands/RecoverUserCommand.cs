using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DLLMobileAPI.Controllers
{
    public class RecoverUserCommand
    {
        public long Cpf { get; set; }

        public string NewPassword { get; set; }
    }
}