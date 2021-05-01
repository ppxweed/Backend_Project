using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using LinkedBack.DTO;
namespace Models
{
    public class SeekersDTO
    {
        public int Seekers_id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Adress { get; set; }
        public string Jobs_exp { get; set; }
        public string Skills {get; set;}
        [RegularExpression("^[A-E]+[a-eA-E]*$", ErrorMessage = "*Rating beetween A to E")]
        public string Rating {get; set;}



    }
}