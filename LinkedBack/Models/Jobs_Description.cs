using System.ComponentModel.DataAnnotations;

namespace LinkedBack.Models
{
    public class Jobs_Description
    {
        [Key]
        public int id { get; set; }
        public int Jobs_id { get; set; }
        public int Salary { get; set; }
        public string Skills_required { get; set; }
        public int Employers_id { get; set; }
    }
}