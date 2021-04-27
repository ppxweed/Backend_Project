using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class EmailDTO
    {
        [Required]
        public List<string> Mails {get; set;}
        public string Mails_Subj { get; set; }
        [Required]
        public string Mails_Body { get; set; }
    }
}