using System;
namespace Models
{
    public class User
    {        
        public int id { get; set; }
        public DateTime Alive { get; set; }
        public DateTime LastSeen { get; set; }
        
        public string Cool_PWD { get; set; }
        public string Mails { get; set; }
        
        public string Level { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}