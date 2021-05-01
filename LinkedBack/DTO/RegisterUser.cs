using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class RegisterUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Mails { get; set; }

        [Required]
        public string Cool_pwd { get; set; }
    }
}