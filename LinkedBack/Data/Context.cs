using Microsoft.EntityFrameworkCore;
using Models;
using LinkedBack.Models;

namespace LinkedBack.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        
         public DbSet<Jobs> Jobs {get; set;}
         

        public DbSet<Jobs_Description> Jobs_Description {get; set;}

        public DbSet<Employers> Employers {get; set;}
        public DbSet<Seekers> Seekers {get; set;}
        public DbSet<Employers_Description> Employers_Description {get; set;}
        public DbSet<Seekers_Description> Seekers_Description {get; set;}


        public DbSet<jobs_list> Jobs_list {get; set;}


        public DbSet<User> User {get; set;}

        public DbSet<Mail> Mail {get;set;}
    }
}