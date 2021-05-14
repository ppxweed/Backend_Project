using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class MailDTO
    {
        [Required]
        public string nameSender { get; set; }

        [Required]
        public string nameReceiver { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string mail { get; set; }
    }
}
