using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Password
    {
        [Required]
        public string Mails { get; set; }
    }
}