using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Optimisation;
using Models;
using LinkedBack.Data;

namespace Mails
{
        public interface IUser
    {
        User Login(string mails, string cool_pwd);
        IEnumerable<User> GetAllOfThem();
        User GetById(int id);
        User NewUser(User login, string cool_pwd , string lvl);
        void Update_profile(User login, string current_cool_pwd, string cool_pwd, string check_cool_pwd);
        string ForgottenPwd(string mails);
        void Delete_Account(int id);
    }

    public class UserDevices : IUser
    {
        private Context _context;
        private readonly Emails _mailDevices;

        public UserDevices(Context ctxt, Emails email)
        {
            _context = ctxt;
            _mailDevices = email;
        }

        public User Login(string mails, string cool_pwd)
        {
            if (string.IsNullOrEmpty(mails) || string.IsNullOrEmpty(cool_pwd))
            {
                return (null);
            }

            
            var log = _context.User.FirstOrDefault(x => x.Mails == mails) ?? null;

            if (log == null)
            {
                return null;
            }

            if (mails == null)
            {
                return (null);
            }

            if(CodedPWD(cool_pwd) != log.Cool_PWD)
            {
                return null;
            }
            return log;        
        }
    
        public IEnumerable<User> GetAllOfThem()
        {
            return _context.User;
        }

        public User GetById(int id)
        {
            return _context.User.Find(id);
        }

        public User NewUser(User mail, string cool_pwd, string lvl)
        {
            if (string.IsNullOrWhiteSpace(cool_pwd))
            {
                throw new Verification("Password is required please can you fill it");
            }

            if (_context.User.Any(x => x.Mails == mail.Mails))
            {
                throw new Verification("This mail \"" + mail.Mails + "\" is already taken");
            }
            mail.Cool_PWD = CodedPWD(cool_pwd);  
            mail.Level = lvl;
            mail.Alive = DateTime.UtcNow;
            mail.LastSeen = DateTime.UtcNow;
            if(mail.Level == "Admin" )
            {
                throw new Verification("Level access denied only Employers or Seekers required");
            }
            else if( mail.Level == "Employers" || mail.Level == "Seekers" )
            {
                _context.User.Add(mail);
                _context.SaveChanges();

                return mail;
                
            }
            throw new Verification("Level access denied only Employers or Seekers required");
        }

        public void Update_profile(User Parameter, string current_cool_pwd = null, string cool_pwd = null, string check_cool_pwd = null)
        {

            var mail = _context.User.Find(Parameter.id);

            if (!string.IsNullOrWhiteSpace(current_cool_pwd))
            {   
                if(CodedPWD(current_cool_pwd) != mail.Cool_PWD)
                {
                    throw new Verification("Invalid Current cool password! For your own good");
                }

                if(current_cool_pwd == cool_pwd)
                {
                    throw new Verification("Please choose another cool password!");
                }

                mail.Cool_PWD = CodedPWD(cool_pwd);
                mail.LastSeen = DateTime.UtcNow; 
            }

            if (!(mail != null))
            {
                throw new Verification("Mail not found, are you sure ?");
            }

             if (!string.IsNullOrWhiteSpace(Parameter.LastName))
            {
                mail.LastName = Parameter.LastName;
                mail.LastSeen = DateTime.UtcNow;
            }
            if (!string.IsNullOrWhiteSpace(Parameter.FirstName))
            {
                mail.FirstName = Parameter.FirstName;
                mail.LastSeen = DateTime.UtcNow;
            }
           

            if (!string.IsNullOrWhiteSpace(Parameter.Mails) && Parameter.Mails != mail.Mails)
            {

                if (_context.User.Any(x => x.Mails == Parameter.Mails))
                {
                    throw new Verification("The Mail " + Parameter.Mails + " is already taken, bad luck i guess");
                }
                else
                {
                    mail.Mails = Parameter.Mails;
                    mail.LastSeen = DateTime.UtcNow;
                }
            }
            

            //DO NOT FORGET THE UPDATE & CHANGES
            
            _context.User.Update(mail);
            _context.SaveChanges();
        }

        public void Delete_Account(int id)
        {
            var mail = _context.User.Find(id);
            if (mail != null)
            {
                _context.User.Remove(mail);
                _context.SaveChanges();
            }
        }

        private static string CodedPWD(string The_Word)
        {
            MD5 crypto = new MD5CryptoServiceProvider();
            var test = crypto.ComputeHash(Encoding.UTF8.GetBytes(The_Word));
            var cesar = "";
            foreach(var cesarus in test)
            {
                cesar += cesarus.ToString("x2");  //LET BE 2 , 3 is too big and 1,5 too short
            } 
            return cesar;
        }

        public string ForgottenPwd(string account)
        {
            if(string.IsNullOrEmpty(account))
            {
                throw new Verification(" A valid Account is requred, register first or try again");
            }
            else
            {
                var mail = _context.User.SingleOrDefault(x => x.Mails == account);
                if(mail != null)
                {
                    string key = CesarKey(5);
                    mail.Cool_PWD = CodedPWD(key);
                    mail.LastSeen = DateTime.UtcNow;
                    _context.SaveChanges();
                    
                    var Address = new List<string>(){account};
                    var Subj = "Password Recovery becareful next time, it's important to register it";
                    var Body = key;

                    var answer = _mailDevices.SendMail(Address,Subj,Body);
                    System.Console.WriteLine(answer.Result.StatusCode);

                    if(answer.IsCompletedSuccessfully)
                    {
                        return new string("You're going to receive an email with your new password");
                    }
                }
                return new string("Your email does't exist please try again");
            }
        }

        private static string CesarKey(int Length_word)
        {
            char[] Specials = @"!#$%&*@\/^-_+=".ToCharArray();
            char[] Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();


            Random randomaseur = new Random();
            int randomSpecial = randomaseur.Next(0,Specials.Length -1);
            int randomUpper = randomaseur.Next(0,Uppercase.Length -1);


            RNGCryptoServiceProvider Crypto = new RNGCryptoServiceProvider();
            byte[] index = new byte[Length_word];
            Crypto.GetBytes(index);
            
            
            string stringo = "";
            foreach(var bytes in index)
            {
                stringo += bytes.ToString("x2"); 
            }
            return Uppercase[randomUpper] + stringo + Specials[randomSpecial];
        }
    }
}