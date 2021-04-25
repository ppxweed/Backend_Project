using System.Collections.Generic;
using Models;
using LinkedBack.DTO;

namespace DTO
{
    
    public class SeekersProfileDTO : SeekersDTO
    {
        public List<JobDTO> Job { get; set; }
    }
}