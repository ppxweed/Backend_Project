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

namespace backend_database_HTTP_Requests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployersController : ControllerBase
    {
        private readonly Context _context;
        public EmployersController(Context context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin, Employers, Seekers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployersDTO>>> GetEmployers()
        {
            var employer = from employers in _context.Employers join Employers_description in _context.Employers_Description on employers.id equals Employers_description.Employers_id
            select new EmployersDTO {
                Employers_id = employers.id,
                User_id = employers.user_id,
                Age = Employers_description.Age,
                Name = employers.Name,
                Job = Employers_description.Job,
                Country = Employers_description.Country,
                Entreprise = employers.Entreprise,
                Rating = Employers_description.Rating
            };

            return await employer.ToListAsync();
        }
        [Authorize(Roles = "Admin, Employers, Seekers")]
        [HttpGet("{id}")]
        public ActionResult<EmployersDTO> GetEmployer_byId(int id)
        {
            var job = from jobs in _context.Jobs
            join job_descriptions in _context.Jobs_Description on jobs.id equals job_descriptions.Jobs_id
            join jobs_list in _context.Jobs_list on jobs.id equals jobs_list.Jobs_id
            select new JobDTO
            {

                Jobs_id = jobs.id,
                Name = jobs.Name,
                Salary = job_descriptions.Salary,
                Skills_required = job_descriptions.Skills_required,
                In_Progress = jobs_list.In_Progress,
                Work_Done = jobs_list.Work_Done,
                Employers_id = jobs_list.Employers_id,
                id = jobs_list.id,
                People_work = jobs_list.People_work
            };

            var employer = from employers in _context.Employers 
            join Employers_description in _context.Employers_Description on employers.id equals Employers_description.Employers_id
            join jobs_list in _context.Jobs_list on employers.id equals jobs_list.Jobs_id
            select new EmployersProfileDTO
            {
                Employers_id = employers.id,
                Age = Employers_description.Age,
                Name = employers.Name,
                Job = Employers_description.Job,
                Country = Employers_description.Country,
                Entreprise = employers.Entreprise,
                Rating = Employers_description.Rating,
                User_id = employers.user_id,
                Jobs = job.Where(x => x.Jobs_id== jobs_list.Jobs_id).ToList()
            };

            var employer_by_id = employer.ToList().Find(x => x.Employers_id == id);

            if (employer_by_id == null)
            {
                return NotFound();
            }
            return employer_by_id;
        }

        [Authorize(Roles = "Admin, Employers")]
        [HttpPost]
         public async Task<ActionResult> Add_Employers(AddEmployers employerDTO)
         {
             if (!ModelState.IsValid)
         {
                 return BadRequest(ModelState);
             }

             var employer = new Employers()
             {
                 Name = employerDTO.Name,
                 Entreprise = employerDTO.Entreprise
             };
             await _context.Employers.AddAsync(employer);
             await _context.SaveChangesAsync();

             var employer_profile = new Employers_Description()
             {
                 Employers_id = employer.id,
                 Job = employerDTO.Job,
                 Age = employerDTO.Age,
                 Country = employerDTO.Country
             };
             await _context.AddAsync(employer_profile);

             await _context.SaveChangesAsync();

             return CreatedAtAction("GetEmployers", new { id = employer.id }, employerDTO);
         }

        [Authorize(Roles = "Admin")]
         [HttpDelete("{id}")]
         public async Task<ActionResult<Employers>> Delete_Employer(int id)
         {
             var employer = _context.Employers.Find(id);
             var employer_profile = _context.Employers_Description.SingleOrDefault(x => x.Employers_id == id);

             if (employer == null)
             {
                 return NotFound();
             }
             else
             {
                 _context.Remove(employer);
                 _context.Remove(employer_profile);
                 await _context.SaveChangesAsync();
                 return employer;
             }
         }
        [Authorize(Roles = Level.Admin)]
        [Authorize(Roles = Level.Employers)]
        [HttpPut("{id}")]
         public async Task<ActionResult> Update_Employers(int id, EmployersDTO employer)
         {
             if (id != employer.Employers_id || !EmployerExists(id))
             {
                 return BadRequest();
             }
             else
             {
                 var employers = _context.Employers.SingleOrDefault(x => x.id == id);
                 var employer_profile = _context.Employers_Description.SingleOrDefault(x => x.Employers_id == id);
                 employers.id = employer_profile.Employers_id;
                 employers.Name = employer.Name;
                 employers.Entreprise = employer.Entreprise;
                 employer_profile.Age = employer_profile.Age;
                 employer_profile.Job = employer_profile.Job;
                 employer_profile.Country = employer_profile.Country;
                 employer_profile.Rating = employer_profile.Rating;
                 await _context.SaveChangesAsync();
                 return NoContent();
             }
         }

         private bool EmployerExists(int id)
         {
             return _context.Employers.Any(x => x.id == id);
         }
    }
}