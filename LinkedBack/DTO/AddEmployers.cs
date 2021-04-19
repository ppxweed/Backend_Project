using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class AddEmployers
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Entreprise { get; set; }
        [Required]
        public string Job { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Country { get; set; }
    }
}