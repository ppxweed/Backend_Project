using System.Collections.Generic;
using System.Threading.Tasks;
using LinkedBack.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using AutoMapper;
using System.Linq;
using DTO;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailsController : ControllerBase
    {
        private readonly Context _context;
        private IMapper _mapper;
        public MailsController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetMails()
        {
            var message = _context.Mail.ToList();
            var model = _mapper.Map<IList<MailDTO>>(message);
            return Ok(model);
        }

        [HttpPost("send_message")]
        public async Task<ActionResult<Mail>> PostMail(Mail message)
        {
            _context.Mail.Add(message);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var text in _context.Mail) { b++; }

            return CreatedAtAction("PostMail", new { id = b }, message);
        }

        [HttpGet("my_messages")]
        public IActionResult Get_myMail(string msg)
        {
            var message = _context.Mail.ToList();
            List<Mail> listmail = new List<Mail>();
            foreach (var item in message)
            {
                if (item.nameSender == msg || item.nameReceiver == msg) { listmail.Add(item); }
            }
            var model = _mapper.Map<IList<MailDTO>>(listmail);
            return Ok(model);
        }

        [HttpGet("my_sent_messages")]
        public IActionResult Get_sentMails(string msgs)
        {
            var msg = _context.Mail.ToList();
            List<Mail> listmail = new List<Mail>();
            foreach (var item in msg)
            {
                if (item.nameSender == msgs) { listmail.Add(item); }
            }
            var model = _mapper.Map<IList<MailDTO>>(listmail);
            return Ok(model);
        }

        [HttpGet("my_received_messages")]
        public IActionResult Get_receivedMails(string msgs)
        {
            var msg = _context.Mail.ToList();
            List<Mail> listmail = new List<Mail>();
            foreach (var item in msg)
            {
                if (item.nameReceiver == msgs) { listmail.Add(item); }
            }
            var model = _mapper.Map<IList<MailDTO>>(listmail);
            return Ok(model);
        }
    }
}
