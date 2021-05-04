using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class AddSeekers
    {
        [Required]
        public int user_id { get; set; }
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
    }
}