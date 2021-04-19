using System.ComponentModel.DataAnnotations;

namespace LinkedBack.Models
{
    public class Rating_employers
    {
        [Key]
        public int id { get; set; }
        public int Employers_id { get; set; }
        [RegularExpression(@"^[A-E]+[a-eA-E]*$")]
        public string Rating { get; set; }
    }
}