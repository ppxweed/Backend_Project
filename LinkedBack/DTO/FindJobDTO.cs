using System.Collections.Generic;

namespace DTO
{
    public class FindJobDTO
    {
        public int Employers_id { get; set; }
        public int Seekers_id {get; set;}
        public List<int> Job {get; set;}
    }
}