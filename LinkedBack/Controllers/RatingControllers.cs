using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using LinkedBack.Data;
using LinkedBack.DTO;
using Microsoft.AspNetCore.Authorization;

namespace LinkedBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly Context _context;
        public RatingController(Context context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet("employers'Rate")]
        public async Task<ActionResult<IEnumerable<EmployersDTO>>> GetEmployers()
        {
            var employer = from employers in _context.Employers join Employers_description in _context.Employers_Description on employers.id equals Employers_description.Employers_id
            select new EmployersDTO {
                Employer_id = employers.id,
                Rating = Employers_description.Rating
            };

            return await employer.ToListAsync();
        }
        [Authorize(Roles = "Admin, Seekers")]
        [HttpPut("employers_{id}")]
         public async Task<ActionResult> Update_Employers(int id, EmployersDTO employer)
         {
             if (id != employer.Employer_id || !EmployerExists(id))
             {
                 return BadRequest();
             }
             else
             {
                 var employer_profile = _context.Employers_Description.SingleOrDefault(x => x.Employers_id == id);
                 employer_profile.Rating = employer.Rating;
                 await _context.SaveChangesAsync();
                 return NoContent();
             }
         }
         private bool EmployerExists(int id)
         {
             return _context.Employers.Any(x => x.id == id);
         }
        [AllowAnonymous]
        [HttpGet("seekers'Rate")]
        public async Task<ActionResult<IEnumerable<SeekersDTO>>> GetSeekers()
        {
            var seek = from seeks in _context.Seekers join seekers_description in _context.Seekers_Description on seeks.id equals seekers_description.Seekers_id
            select new SeekersDTO {
                Seekers_id = seeks.id,
                Rating = seekers_description.Rating
            };

            return await seek.ToListAsync();
        }
        [Authorize(Roles = "Admin, Employers")]
        [HttpPut("seekers_{id}")]
         public async Task<ActionResult> Update_Seekers(int id, SeekersDTO seek)
         {
             if (id != seek.Seekers_id || !SeekersExists(id))
             {
                 return BadRequest();
             }
             else
             {
                 var seeks = _context.Seekers.SingleOrDefault(x => x.id == id);
                 var seek_profile = _context.Seekers_Description.SingleOrDefault(x => x.Seekers_id == id);
                 seek_profile.Rating = seek.Rating;
                 await _context.SaveChangesAsync();
                 return NoContent();
             }
         }
         private bool SeekersExists(int id)
         {
             return _context.Seekers.Any(x => x.id == id);
         }
    }
    
}