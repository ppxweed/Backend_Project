using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTO;
using Optimisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using Mails;
using LinkedBack.Data;

namespace Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUser _UserDevices;
        private IMapper _map;
        public IConfiguration _Config;

        private readonly Context _ctxt;

        private readonly Emails _mailDevices;

        public UsersController(
            Context ctxt,
            IUser UserDevices,
            IMapper map,
            IConfiguration config,
            Emails mailDevices)
        {
            _ctxt = ctxt;
            _UserDevices = UserDevices;
            _map = map;
            _Config = config;
            _mailDevices = mailDevices;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult SignIn([FromBody]Login log)
        {
            var user = _UserDevices.Login(log.Mail, log.Cool_pwd);

            if (!(user != null))
                return BadRequest(new { message = "Mails or The cool_password is incorrect, please try again" });

            var tokenBEARER = new JwtSecurityTokenHandler();
            var key_cesar = Encoding.ASCII.GetBytes(_Config["Secret"]);
            var tokenCrypto = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] //WE CAN'T CHANGE THE NAME or THE ROLE so deal with it
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.Level ?? "null")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key_cesar), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenBEARER.CreateToken(tokenCrypto);
            var tokenS = tokenBEARER.WriteToken(token);
            return Ok(new
            {
                id = user.id,
                Mails = user.Mails,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenS
            });

            


            

           
        }

        //[Authorize(Roles = Level.Admin)]
        [HttpPost("level/{id}")]
        public IActionResult ChangeLevel(int id, UpdateLevelDTO lvl)
        {
            //Always keep the admin for one person
            _ctxt.User.Find(id).Level = lvl.Level;
            _ctxt.SaveChanges();
            return Ok("The level has been updated! Keep an eye on it");
        }


        [AllowAnonymous]
        [HttpPost("SignIn")]
        public IActionResult Reg([FromBody]RegisterUser index)
        {
            var people = _map.Map<User>(index);

            try
            {
                _UserDevices.NewUser(people, index.Cool_pwd);
                return Ok();
            }
            catch (Verification error)
            {
                return BadRequest(new { message = error.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllOf()
        {
            var people = _UserDevices.GetAllOfThem();
            var index = _map.Map<IList<User_User>>(people);
            return Ok(index);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var people = _UserDevices.GetById(id);
            var index = _map.Map<User_User>(people);
            return Ok(index);
        }

        //[Authorize(Roles = Level.Admin)]
        [HttpPost("mail")]
        public async Task<IActionResult> SendEmail(EmailDTO index)
        {
            var mails = new List<string>();
            foreach (var items in index.Mails)
            {
                mails.Add(items);
            }

            var answer = await _mailDevices.SendMail(mails, index.Mails_Subj, index.Mails_Body);

            if (answer.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
               return Ok("Email sent " + answer.StatusCode);
            }
            else
            {
                return BadRequest("Email sending failed " + answer.StatusCode);
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot_your_password")]
        public IActionResult ForgotPassword(Password index)
        {
            return Ok(_UserDevices.ForgottenPwd(index.Mails));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UpdateUser index)
        {
            int log = int.Parse(User.Identity.Name);

            var people = _map.Map<User>(index);
            people.id = id;


            if(log != id)
            {
                return BadRequest(new { msg = "Access Denied please try again with another id" });
            }

            try
            {
                _UserDevices.Update_profile(people, index.Current_Cool_pwd, index.New_Cool_pwd, index.CheckNew_Cool_pwd);
                return Ok();
            }
            catch (Verification error)
            {
                return BadRequest(new { msg = error.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _UserDevices.Delete_Account(id);
            return Ok();
        }

        
    }
}