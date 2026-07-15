using AutoMapper;
using UserControl.Core.DTOs;
using UserControl.Core.Entities;
using UserControl.Application.Responses;

namespace UserControl.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();

            CreateMap<Security, SecurityDto>().ReverseMap();


            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<ApiResponses<User>, ApiResponses<UserDto>>().ReverseMap();

        }
    }
}
