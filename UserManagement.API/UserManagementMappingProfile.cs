using AutoMapper;
using UserManagement.API.DataAccess.Entities;
using UserManagement.API.Models;

namespace UserManagement.API
{
    public class UserManagementMappingProfile : Profile
    {
        public UserManagementMappingProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, RegisterUserViewModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()).ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<Company, CompanyViewModel>().ReverseMap();
        }
    }
}
