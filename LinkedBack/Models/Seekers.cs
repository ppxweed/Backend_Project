using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Seekers
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public int user_id {get;set;}

    }
}