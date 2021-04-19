using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Employers_Description
    {
        [Key]
        public int id { get; set; }
        public int Employers_id { get; set; }
        public string Job { get; set; }
        public string Country { get; set; }
        public int Age {get; set;}
    }
}