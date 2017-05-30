using System;

namespace DLLMobileAPI
{
    public class LoginActivity
    {
        public int Id { get; set; }

        public int IdUser { get; set; }

        public virtual ApplicationUser User { get; set; }
        
        public string DeviceId { get; set; }

        public DateTime LoginDate { get; set; }
    }
}