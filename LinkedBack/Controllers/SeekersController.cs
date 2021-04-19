using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using LinkedBack.Data;
using LinkedBack.DTO;

namespace backend_database_HTTP_Requests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeekersController : ControllerBase
    {
        private readonly Context _context;
        public SeekersController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeekersDTO>>> GetSeekers()
        {
            var seek = from seeks in _context.Seekers join seekers_description in _context.Seekers_Description on Seekers.id equals seekers_description.seeks_id
            select new SeekersDTO {
                Seekers_id = seeks.id,
                Age = seekers_description.Age,
                Name = seeks.Name,
                Job_exp = seekers_description.Job_exp,
                Adress = seekers_description.Adress,
                Skills = seeks.Skills
            };

            return await employer.ToListAsync();
        }

        [HttpGet("{id}")]
        public ActionResult<SeekersDTO> GetSeekers_byId(int id)
        {
            var job = from jobs in _context.Jobs
            join job_descriptions in _context.Jobs_Description on jobs.id equals job_descriptions.Jobs_id
            join jobs_list in _context.Jobs_list on jobs.id equals jobs_list.Jobs_id
            select new JobsDTO
            {

                Jobs_id = jobs.id,
                Name = jobs.Name,
                Jobs_id_id = job_descriptions.Jobs_id,
                Salary = job_descriptions.Salary,
                Skills_required = job_descriptions.Skills_required,
                Allocation_date = jobs_list.Allocation_date,
                Return_date = jobs_list.Return_date,
                Employers_id = jobs_list.Employers_id,
                id = jobs_list.id,
                Renewed = jobs_list.Renewed
            };
            var seek = from seeks in _context.Seekers join seekers_description in _context.Seekers_Description on Seekers.id equals seekers_description.seeks_id
            join jobs_list in _context.Jobs_list on seeks.id equals jobs_list.Jobs_id
            select new SeekersProfileDTO
            {
                Seekers_id = seeks.id,
                Age = seekers_description.Age,
                Name = seeks.Name,
                Job_exp = seekers_description.Job_exp,
                Adress = seekers_description.Adress,
                Skills = seekers_description.Skills,
                Jobs = job.Where(x => x.Jobs_id== jobs_list.Jobs_id).ToList()
            };

            var seek_by_id = seek.ToList().Find(x => x.seeks_id == id);

            if (seek_by_id == null)
            {
                return NotFound();
            }
            return seek_by_id;
        }


             [HttpPost]
         public async Task<ActionResult> Add_Seeks(AddSeekers seekerDTO)
         {
             if (!ModelState.IsValid)
         {
                 return BadRequest(ModelState);
             }

             var seek = new Seekers()
             {
                 Name = seekerDTO.Name
             };
             await _context.Seekers.AddAsync(seek);
             await _context.SaveChangesAsync();

             var seek_profile = new Seekers_Description()
             {
                 seekId = seek.id,
                 Job_exp = seekDTO.Job_exp,
                 Age = seekDTO.Age,
                 Adress = seekDTO.Adress
             };
             await _context.AddAsync(seek_profile);

             await _context.SaveChangesAsync();

             return CreatedAtAction("GetSeekers", new { id = seek.id }, seekerDTO);
         }


         [HttpDelete("{id}")]
         public async Task<ActionResult<Seekers>> Delete_Seekers(int id)
         {
             var seek = _context.Seekers.Find(id);
             var seek_profile = _context.Seekers_Description.SingleOrDefault(x => x.seekId == id);

             if (seek == null)
             {
                 return NotFound();
             }
             else
             {
                 _context.Remove(seek);
                 _context.Remove(seek_profile);
                 await _context.SaveChangesAsync();
                 return seek;
             }
         }

         [HttpPut("{id}")]
         public async Task<ActionResult> Update_Seekers(int id, SeekersDTO seek)
         {
             if (id != seek.seekId || !EmployerExists(id))
             {
                 return BadRequest();
             }
             else
             {
                 var seeks = _context.Seekers.SingleOrDefault(x => x.id == id);
                 var seek_profile = _context.Seekers_Description.SingleOrDefault(x => x.seekId == id);
                 seeks.id = seek.seekId;
                 seeks.Name = seek.Name;
                 seek_profile.Age = seek.Age;
                 seek_profile.Job_exp = employer.Job_exp;
                 seek_profile.Adress = seek.Adress;
                 seek_profile.Skills = seek.Skills;
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