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
    [Authorize(Roles = "Admin, Support")]
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
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobs()
        {
            var job = from jobs in _context.Jobs
            join jobs_descriptions in _context.Jobs_Description on jobs.id equals jobs_descriptions.Jobs_id
            select new JobDTO
            {
                Jobs_id = jobs.id,
                Salary = jobs_descriptions.Salary,
                Name = jobs.Name,
                Entreprise = jobs_descriptions.Entreprise,
                Skills_required = jobs_descriptions.Skills_required
            };

            return await job.ToListAsync();
        }

        [HttpGet("{id}")]
        public ActionResult<JobDTO> GetJobs_byId(int id)
        {
            
            var job = from jobs in _context.Jobs
            join jobs_descriptions in _context.Jobs_Description on jobs.id equals jobs_descriptions.Jobs_id
            select new JobDTO
            {
                Jobs_id = jobs.id,
                Salary = jobs_descriptions.Salary,
                Name = jobs.Name,
                Entreprise = jobs_descriptions.Entreprise,
                Skills_required = jobs_descriptions.Skills_required
            };

            var jobs_by_id = job.ToList().Find(x => x.Jobs_id == id);

            if (jobs_by_id == null)
            {
                return NotFound();
            }
            return jobs_by_id;
        }

        [HttpPost]
        public async Task<ActionResult<AddJob>> Add_Jobs(AddJob jobDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var job = new Jobs()
            {
                name = jobDTO.Name
            };
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();

            var jobs_description = new Jobs_Description()
            {
                job_id = job.id,
                job_name = jobDTO.Name,
                Salary = jobDTO.Salary,
                Skills_required = jobDTO.Skills_required
            };
            await _context.AddAsync(jobs_description);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobs", new { id = job.id}, jobDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Jobs>> Delete_Jobs(int id)
        {
            var job = _context.Jobs.Find(id);
            var job_description = _context.Jobs_Description.SingleOrDefault(x => x.job_id == id);

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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update_Jobs(int id, JobDTO jobs)
        {
            if(id != jobs.Book_id || !JobExists(id))
            {
                return BadRequest();
            }
            else 
            {
                var job = _context.Jobs.SingleOrDefault(x => x.id == id);
                var job_description = _context.Jobs_Description.SingleOrDefault(x => x.job_id == id);


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