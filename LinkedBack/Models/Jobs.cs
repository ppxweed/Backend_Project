using System.ComponentModel.DataAnnotations;

namespace LinkedBack.Models
{
    public class Jobs
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
    }
}