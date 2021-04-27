using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Login
    {
        [Required]
        public string Mail { get; set; }

        [Required]
        public string Cool_pwd { get; set; }
    }
}