using Airport.Client.DTOs;
using Airport.Data.Models;
using AutoMapper;

namespace Airport.Client.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Airplane, AirplaneDTO>();
            CreateMap<Terminal, TerminalDTO>();
            CreateMap<Flight, FlightDTO>();
        }
    }
}
