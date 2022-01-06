using AutoMapper;
using iqrasys.api.Dtos;
using iqrasys.api.Models;

namespace iqrasys.api.Helpers
{

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserForReturnDto>();
            CreateMap<User, ArchiveUser>().ForMember(u => u.Id, opt => opt.Ignore());
        }
    }

}