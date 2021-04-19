using System.Collections.Generic;
using Models;
using LinkedBack.DTO;

namespace DTO
{
    public class EmployersProfileDTO : EmployersDTO
    {
        public List<JobDTO> Job { get; set; }
    }
}