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
    public class EmployersController : ControllerBase
    {
        private readonly Context _context;
        public EmployersController(Context context)
        {
            _context = context;
        }
        //[Authorize(Roles = "Admin, Employers, Seekers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployersDTO>>> GetEmployers()
        {
            var employer = from employers in _context.Employers join Employers_description in _context.Employers_Description on employers.id equals Employers_description.Employers_id
            select new EmployersDTO {
                Employer_id = employers.id,
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
        //[Authorize(Roles = "Admin, Employers, Seekers")]
                [HttpGet("{id}")]
        public ActionResult<EmployersDTO> GetEmployer_byId(int id)
        {
            EmployersDTO theEmployer = new EmployersDTO();
            theEmployer.Employer_id = id;
            bool check = true;
            foreach (var s in _context.Employers.ToList())
            {
                if(s.id == id)
                {
                    theEmployer.Name = s.Name;
                    theEmployer.Entreprise = s.Entreprise;
                    theEmployer.User_id = s.user_id;
                    check = false;
                }
            }
            if (check)
            {
                return NotFound();
            }
            foreach (var detail in _context.Employers_Description.ToList())
            {
                if(detail.Employers_id == id)
                {
                    theEmployer.Age = detail.Age;
                    theEmployer.Job = detail.Job;
                    theEmployer.Country = detail.Country;
                    theEmployer.Rating = detail.Rating;
                }
            }
            List<JobDTO> jobList = new List<JobDTO>();
            foreach (var job in _context.Jobs_list.ToList())
            {
                if(job.Employers_id == id)
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
            theEmployer.Jobs = jobList;
            return theEmployer;
        }

        //[Authorize(Roles = "Admin, Employers")]
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
                 Entreprise = employerDTO.Entreprise,
                 user_id = employerDTO.user_id
             };
             await _context.Employers.AddAsync(employer);
             await _context.SaveChangesAsync();

             var employer_profile = new Employers_Description()
             {
                 Employers_id = employer.id,
                 Job = employerDTO.Job,
                 Age = employerDTO.Age,
                 Country = employerDTO.Country,
                 Rating = ""
             };
             await _context.AddAsync(employer_profile);

             await _context.SaveChangesAsync();

             return CreatedAtAction("GetEmployers", new { id = employer.id }, employerDTO);
         }

        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin, Employers")]
        [HttpPut("{id}")]
         public async Task<ActionResult> Update_Employers(int id, EmployersDTO employer)
         {
             if (id != employer.Employer_id || !EmployerExists(id))
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
                 employer_profile.Age = employer.Age;
                 employer_profile.Job = employer.Job;
                 employer_profile.Country = employer.Country;
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