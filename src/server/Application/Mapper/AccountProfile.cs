using Application.DTOs.Account;
using AutoMapper;
using Domain.Entities.GoogleOAuth;

namespace Application.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<GoogleTokenResponse, UserTokenDto>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());
        }
    }
}
