using AutoMapper;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Mapper;

public class FilmProfile : Profile
{
    public FilmProfile()
    {
        CreateMap<FilmSaveDTO, Film>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            // .ForMember(
            //     dest => dest.Genre.Name,
            //     opt => opt.MapFrom(src => src.GenreName))
            .ForMember(
                dest => dest.ReleaseDate,
                opt => opt.MapFrom(src => src.ReleaseDate))
            .ForMember(
                dest => dest.DateAdded,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(
                dest => dest.DateUpdated,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(
                dest => dest.DateDelete,
                opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<Film, FilmForOutputDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(x => x.Id))
            .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(x => x.Title))
            .ForMember(
                dest => dest.GenreName,
                opt => opt.MapFrom(x => x.Genre.Name))
            .ForMember(
                dest => dest.ReleaseDate,
                opt => opt.MapFrom(x => x.ReleaseDate));
    }
}