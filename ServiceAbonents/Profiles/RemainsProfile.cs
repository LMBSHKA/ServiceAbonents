using AutoMapper;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Profiles
{
    public class RemainsProfile : Profile
    {
        public RemainsProfile() 
        {
            CreateMap<Remain, RemainReadDto>();
            CreateMap<RemainCreateDto, Remain>();
        }
    }
}
