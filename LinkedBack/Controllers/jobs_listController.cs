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
using Models;
using System;




namespace LinkedBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class jobs_listController : ControllerBase
    {
        private readonly Context _context;

        public jobs_listController(Context context)
        {
            _context = context;
        }

        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<jobs_list>>> GetListJobs()
        {
            return await _context.Jobs_list.ToListAsync();
        }*/

        [HttpPost("FindJob")]
        public async Task<ActionResult<IEnumerable<FindJobDTO>>> FindJobBook(FindJobDTO find)
        {
            foreach(var item in find.Job)
            {
                var list = new jobs_list()
                {
                    Employers_id = find.Employers_id,
                    Seekers_id = find.Seekers_id,
                    Jobs_id= item,
                    In_Progress = DateTime.UtcNow,
                    Work_Done = null,
                    People_work = 0
                };
                await _context.Jobs_list.AddAsync(list);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetListJobs",find);
        }

        [HttpPost("JobDone")]
        public async Task<ActionResult<IEnumerable<FindJobDTO>>> JobDone(ReturnJobDTO work_done)
        {
            foreach(var item in work_done.id)
            {
                var list_item = await _context.Jobs_list.SingleOrDefaultAsync(x => x.Id == item);
                list_item.Work_Done = DateTime.UtcNow;
                list_item.People_work = list_item.People_work + 1;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetListJobs",work_done);
        }

        
    }
}