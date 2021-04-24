using System.ComponentModel.DataAnnotations;
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

    }
}