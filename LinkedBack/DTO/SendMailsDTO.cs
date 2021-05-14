using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class SendMailsDTO
    {
        [Required]
        public List<string> emails { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Mail { get; set; }
    }
}