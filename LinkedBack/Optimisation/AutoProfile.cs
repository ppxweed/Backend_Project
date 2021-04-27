using AutoMapper;
using DTO;
using Models;

namespace Optimisation
{
    public class AutoProfile : Profile
    {
        public AutoProfile()
        {       //Adjustmeent with the mapper style, BECAREFUL not use the thing number 3 ! 
                CreateMap<RegisterUser, User>();
            
            CreateMap<UpdateUser, User>();

            
            CreateMap<User, User_User>();
        }
    }
}