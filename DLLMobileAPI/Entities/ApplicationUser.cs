using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLLMobileAPI
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Index(IsUnique = true)]
        public long Cpf { get; set; }

        public long CellPhoneNumber { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}