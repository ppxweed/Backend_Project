using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class jobs_list
    {
        [Key]
        public int id { get; set; }
        public int Jobs_id { get; set; }
        public int Employers_id { get; set; }
        public int Seekers_id {get; set;}
        public DateTime Allocation_date { get; set; }
        public DateTime? Return_date { get; set; }
        public int Renewed { get; set; }
    }
}