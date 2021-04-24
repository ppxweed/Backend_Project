using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class AddSeekers
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Jobs_exp { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Skills { get; set; }
        [Required]
        public string Rating { get; set; }
    }
}