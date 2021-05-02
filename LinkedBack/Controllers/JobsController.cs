using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LinkedBack.Data;
using LinkedBack.DTO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinkedBack.Models;
using DTO;
using Microsoft.AspNetCore.Authorization;


namespace LinkedBack.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly Context _context;

        public JobsController(Context context)
        {
            _context = context;
        }
        //[Authorize(Roles = "Admin, Employers, Seekers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobs()
        {
            var job = from jobs in _context.Jobs
            join jobs_descriptions in _context.Jobs_Description on jobs.id equals jobs_descriptions.jobs_id
            select new JobDTO
            {
                Jobs_id = jobs.id,
                Salary = jobs_descriptions.Salary,
                Name = jobs.Name,
                Skills_required = jobs_descriptions.Skills_required
            };

            return await job.ToListAsync();
        }
        //[Authorize(Roles = "Admin, Employers, Seekers")]
        [HttpGet("{id}")]
        public ActionResult<JobDTO> GetJobs_byId(int id)
        {
            
            var job = from jobs in _context.Jobs
            join jobs_descriptions in _context.Jobs_Description on jobs.id equals jobs_descriptions.jobs_id
            select new JobDTO
            {
                Jobs_id = jobs.id,
                Salary = jobs_descriptions.Salary,
                Name = jobs.Name,
                Skills_required = jobs_descriptions.Skills_required
            };

            var jobs_by_id = job.ToList().Find(x => x.Jobs_id == id);

            if (jobs_by_id == null)
            {
                return NotFound();
            }
            return jobs_by_id;
        }
        //[Authorize(Roles = "Admin, Employers")]
        [HttpPost]
        public async Task<ActionResult<AddJob>> Add_Jobs(AddJob jobDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var job = new Jobs()
            {
                Name = jobDTO.Name
            };
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();

            var jobs_description = new Jobs_Description()
            {
                jobs_id = job.id,
                Salary = jobDTO.Salary,
                Skills_required = jobDTO.Skills_required,
                Employers_id = jobDTO.Employers_id
            };
            await _context.AddAsync(jobs_description);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobs", new { id = job.id}, jobDTO);
        }
        //[Authorize(Roles = "Admin, Employers")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jobs>> Delete_Jobs(int id)
        {
            var job = _context.Jobs.Find(id);
            var job_description = _context.Jobs_Description.SingleOrDefault(x => x.jobs_id == id);

            if(job == null)
            {
                return NotFound();
            }
            else 
            {
                _context.Remove(job);
                _context.Remove(job_description);
                await _context.SaveChangesAsync();
                return job;
            }
        }
        //[Authorize(Roles = "Admin, Employers")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update_Jobs(int id, JobDTO jobs)
        {
            if(id != jobs.Employers_id || !JobExists(id))
            {
                return BadRequest();
            }
            else 
            {
                var job = _context.Jobs.SingleOrDefault(x => x.id == id);
                var job_description = _context.Jobs_Description.SingleOrDefault(x => x.jobs_id == id);


                job_description.Salary = jobs.Salary;
                job.Name = jobs.Name;
                job_description.Skills_required = jobs.Skills_required;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(x => x.id == id);
        }
    }
}