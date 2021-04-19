using Models;

namespace LinkedBack.DTO
{
    public class JobDTO : jobs_list
    {
        public int Employers_id {get; set;}
        public int Salary { get; set; }
        public string Name { get; set; }
        public string Entreprise { get; set; }
        public string Skills_required { get; set; }
    }
}