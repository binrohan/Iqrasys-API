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

            CreateMap<User, ArchiveUser>()
                .ForMember(u => u.Id, opt => opt.Ignore());

            CreateMap<Message, MailRequest>()
                .ForMember(dest => dest.ToEmail, 
                            opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Body, 
                            opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Subject,
                            opt => opt.MapFrom(src => $"New Message from [{src.Email ?? "No Mail"}]"));

            CreateMap<Request, MailRequest>()
                .ForMember(dest => dest.ToEmail, 
                            opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Body, 
                            opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Subject,
                            opt => opt.MapFrom(src => $"New Message from [{src.Email ?? "No Mail"}]"));
        }
    }

}