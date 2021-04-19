using System.ComponentModel.DataAnnotations;

namespace LinkedBack.Models
{
    public class Rating_seekers
    {
        [Key]
        public int id { get; set; }
        public int Seeker_id { get; set; }
        [RegularExpression(@"^[A-E]+[a-eA-E]*$")]
        public string Rating { get; set; }
        public bool Work {get; set;}
    }
}