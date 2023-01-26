using AutoMapper;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Mapper;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<GenreSaveDTO, Genre>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(
                dest => dest.DateAdded,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(
                dest => dest.DateUpdated,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(
                dest => dest.DateDelete,
                opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<Genre, GenreOutputDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(x => x.Id))
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(x => x.Name))
            .ForMember(
                dest => dest.Films,
                opt => opt.MapFrom(x => x.Films));
    }
}