using DaraSurvey.Entities;
using DaraSurvey.Models;

namespace DaraSurvey.Mappers
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<UserUpdateModel, User>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.User.CountryCode))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForPath(dest => dest.Profile.Image, opt => opt.MapFrom(src => src.Profile.Image))
                .ForPath(dest => dest.Profile.Gender, opt => opt.MapFrom(src => src.Profile.Gender))
                .ForPath(dest => dest.Profile.BirthDate, opt => opt.MapFrom(src => src.Profile.BirthDate))
                .ForPath(dest => dest.Profile.NationalCode, opt => opt.MapFrom(src => src.Profile.NationalCode));

            // --------------------

            CreateMap<User, UserUpdateModel>();

            // --------------------

            CreateMap<RegisterReq, User>();

            // --------------------

            CreateMap<User, UserRes>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created))
                .ForPath(dest => dest.Profile.Gender, opt => opt.MapFrom(src => src.Profile.Gender))
                .ForPath(dest => dest.Profile.BirthDate, opt => opt.MapFrom(src => src.Profile.BirthDate))
                .ForPath(dest => dest.Profile.Image, opt => opt.MapFrom(src => src.Profile.Image))
                .ForPath(dest => dest.Profile.NationalCode, opt => opt.MapFrom(src => src.Profile.NationalCode))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName));

            // --------------------

            CreateMap<ProfileReq, Profile>();

            // --------------------

            CreateMap<UserReq, User>();
        }
    }
}
