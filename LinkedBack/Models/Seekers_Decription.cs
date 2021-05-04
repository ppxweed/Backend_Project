using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Seekers_Description
    {
        [Key]
        public int id { get; set; }
        public int Seekers_id { get; set; }
        public int Age { get; set; }
        public string Adress { get; set; }
        public string Jobs_exp { get; set; }
        public string Skills {get; set;}

        [RegularExpression(@"^[A-E]+[a-eA-E]*$")]
        public string Rating {get; set;}

    }
}