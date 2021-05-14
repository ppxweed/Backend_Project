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
    public class SeekersController : ControllerBase
    {
        private readonly Context _context;
        public SeekersController(Context context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin, Employers, Seekers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeekersDTO>>> GetSeekers()
        {
            var seek = from seeks in _context.Seekers join seekers_description in _context.Seekers_Description on seeks.id equals seekers_description.Seekers_id
            select new SeekersDTO {
                Seekers_id = seeks.id,
                user_id = seeks.user_id,
                Age = seekers_description.Age,
                Name = seeks.Name,
                Jobs_exp = seekers_description.Jobs_exp,
                Adress = seekers_description.Adress,
                Skills = seekers_description.Skills,
                Rating = seekers_description.Rating
            };

            return await seek.ToListAsync();
        }
        [Authorize(Roles = "Admin, Employers, Seekers")]
        [HttpGet("{id}")]
        public ActionResult<SeekersDTO> GetSeekers_byId(int id)
        {   
            SeekersDTO TheSeekers = new SeekersDTO();
            TheSeekers.Seekers_id = id;
            bool check = true;
            foreach (var s in _context.Seekers)
            {
                if(s.id == id)
                {
                    TheSeekers.Name = s.Name;
                    TheSeekers.user_id = s.user_id;
                    check = false;
                }
            }
            if (check)
            {
                return NotFound();
            }
            foreach (Seekers_Description Seek_Des in _context.Seekers_Description)
            {
                if(Seek_Des.Seekers_id == id)
                {
                    TheSeekers.Age = Seek_Des.Age;
                    TheSeekers.Adress =  Seek_Des.Adress;
                    TheSeekers.Jobs_exp = Seek_Des.Jobs_exp;
                    TheSeekers.Skills = Seek_Des.Skills;
                    TheSeekers.Rating = Seek_Des.Rating;
                }
            }
            List<JobDTO> jobList = new List<JobDTO>();
            foreach (var job in _context.Jobs_list.ToList())
            {
                if(job.Seekers_id == id)
                {
                    JobDTO newJob = new JobDTO();
                    foreach(var jobname in _context.Jobs.ToList())
                    {
                        if(job.Jobs_id == jobname.id)
                        {
                            newJob.Name = jobname.Name;
                        }
                    }
                    foreach(var jobdesciption in _context.Jobs_Description.ToList())
                    {
                        if(job.Jobs_id == jobdesciption.jobs_id)
                        {
                            newJob.Salary = jobdesciption.Salary;
                            newJob.Skills_required = jobdesciption.Skills_required;
                        }
                    }
                    foreach(var jobenterprise in _context.Employers.ToList())
                    {
                        if(jobenterprise.id == job.Employers_id)
                        {
                            newJob.Entreprise = jobenterprise.Entreprise;
                        }
                    }
                    jobList.Add(newJob);
                }
            }
            TheSeekers.Jobs = jobList;

            return TheSeekers;
        }

        [Authorize(Roles = "Admin, Seekers")]
        [HttpPost]
         public async Task<ActionResult> Add_Seeks(AddSeekers seekerDTO)
         {
             if (!ModelState.IsValid)
         {
                 return BadRequest(ModelState);
             }

             var seek = new Seekers()
             {
                 Name = seekerDTO.Name,
                 user_id = seekerDTO.user_id
             };
             await _context.Seekers.AddAsync(seek);
             await _context.SaveChangesAsync();

             var seek_profile = new Seekers_Description()
             {
                 Seekers_id = seek.id,
                 Jobs_exp = seekerDTO.Jobs_exp,
                 Age = seekerDTO.Age,
                 Adress = seekerDTO.Adress,
                 Skills = seekerDTO.Skills,
                 Rating = ""
             };
             await _context.AddAsync(seek_profile);

             await _context.SaveChangesAsync();

             return CreatedAtAction("GetSeekers", new { id = seek.id }, seekerDTO);
         }

        [Authorize(Roles = "Admin")]
         [HttpDelete("{id}")]
         public async Task<ActionResult<Seekers>> Delete_Seekers(int id)
         {
             var seek = _context.Seekers.Find(id);
             var seek_profile = _context.Seekers_Description.SingleOrDefault(x => x.Seekers_id == id);

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
        [Authorize(Roles = "Admin, Seekers")]
        [HttpPut("{id}")]
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
                 seeks.id = seek_profile.Seekers_id;
                 seeks.Name = seek.Name;
                 seek_profile.Age = seek.Age;
                 seek_profile.Jobs_exp = seek.Jobs_exp;
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