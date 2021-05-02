using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class AddJob
    {
        [Required]
        public int Employers_id { get; set; }
        [Required]
        public int Jobs_id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Skills_required { get; set; }
        [Required]
        public int Salary { get; set; }
        [Required]
        public string Entreprise { get; set; }
    }
}