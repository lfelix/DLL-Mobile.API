﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DLLMobileAPI.Commands
{
    public class UpdateUserCommand
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }
    }
}