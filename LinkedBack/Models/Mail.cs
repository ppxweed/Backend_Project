using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Mail
    {
        [Key]
        public int id { get; set; }
        public string nameSender { get; set; }
        public string nameReceiver { get; set; }
        public string subject { get; set; }
        public string mail { get; set; }
    }
}