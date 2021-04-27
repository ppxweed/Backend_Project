using System.ComponentModel.DataAnnotations;
namespace DTO
{
    public class UpdateUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Mails { get; set; }
        
        public string Current_Cool_pwd { get; set; }
        [DataType(DataType.Password)]
        public string New_Cool_pwd { get; set; }
        [DataType(DataType.Password)]
        public string CheckNew_Cool_pwd { get; set; }
    }
}