using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using LinkedBack.DTO;
namespace Models
{
    public class EmployersDTO 
    {
        public int Employers_id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Job {get; set;}
        public string Country { get; set; }
        public string Entreprise {get; set;}
        [RegularExpression("^[A-E]+[a-eA-E]*$", ErrorMessage = "*Rating beetween A to E")]
        public string Rating {get; set;}

        public int User_id {get;set;}

        public List<JobDTO> Jobs {get; set;}
    }
}