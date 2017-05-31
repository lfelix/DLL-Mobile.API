using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLLMobileAPI
{
    public class LoginActivity
    {
        public int Id { get; set; }

        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }
        
        public string DeviceId { get; set; }

        public DateTime LoginDate { get; set; }

        public string Location { get; set; }

        public string FriendlyDeviceName { get; set; }
    }
}