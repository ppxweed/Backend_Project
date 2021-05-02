using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class jobs_list
    {
        [Key]
        public int Id { get; set; }
        public int Jobs_id { get; set; }
        public int Employers_id { get; set; }
        public int Seekers_id {get; set;}
        public DateTime In_Progress { get; set; }
        public DateTime? Work_Done { get; set; }
        public int People_work { get; set; }

    }
}