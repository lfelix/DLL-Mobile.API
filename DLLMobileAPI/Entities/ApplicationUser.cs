﻿using System.ComponentModel.DataAnnotations;

namespace DLLMobileAPI
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}