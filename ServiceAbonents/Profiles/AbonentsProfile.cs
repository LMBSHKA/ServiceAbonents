using AutoMapper;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;
using System.Runtime.InteropServices;

namespace ServiceAbonents.Profiles
{
    public class AbonentsProfile : Profile
    {
        public AbonentsProfile() 
        {
            CreateMap<Abonent, AbonentReadDto>();
            CreateMap<AbonentCreateDto, Abonent>();
        }
    }
}
