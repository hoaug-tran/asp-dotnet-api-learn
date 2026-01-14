using AutoMapper;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs;
using Microsoft.AspNetCore.Identity.Data;

namespace LearnLinQWeb.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<User, LoginResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<Book, BookResponse>();
        CreateMap<CreateBookRequest, Book>();
        CreateMap<UpdateBookRequest, Book>();
    }
}
