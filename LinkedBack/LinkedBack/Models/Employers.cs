using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Employers
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string Entreprise {get; set;}

    }
}