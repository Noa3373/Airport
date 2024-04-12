using Airport.Client.DTOs;
using Airport.Client.Hubs;
using Airport.Data.Models;
using Airport.Data.Repository;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Airport.Client.Services
{
    public class FlightService
    {
        private readonly IRepository<Flight> _flightRepository;
        private IRepository<Terminal> _terminalRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<FlightHub> _hubContext;

        public FlightService(IRepository<Flight> flightRepository,
                             IRepository<Terminal> terminalRepository,
                             IMapper mapper,
                             IHubContext<FlightHub> hubContext)
        {
            _flightRepository = flightRepository;
            _terminalRepository = terminalRepository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task StartAirportSimulation(Airplane newAirplane)
        {

            var airplane = newAirplane;
            while (airplane.CurrentTerminal != 9)
            {
                var flight = await SetFlight(airplane);
                await CheckPermisionToFly(flight); 
                await MakeFlight(flight);
            }

        }

        private async Task<Flight> SetFlight(Airplane airplane)
        {
            Flight flight = new Flight
            {
                Airplane = airplane,
                Terminal = SetTerminal(airplane)
            };

            await _flightRepository.Add(flight);
            await ScheduleFlight(flight);

            return flight;
        }

        private async Task ScheduleFlight(Flight flight)
        {
            ScheduledFlight sf = new ScheduledFlight
            {
                FlightId = flight.Id,
                ScheduledArrivalTime = SetScheduledArrivalTime(flight),
                ScheduledDepartureTime = SetScheduledDepartureTime(flight)
            };

            flight.ScheduledFlights.Add(sf);
            await _flightRepository.Update(flight);
        }

        private async Task CheckPermisionToFly(Flight flight)
        {
            //if (flight.Airplane.CurrentTerminal != null)
            //    await UpdatePreFlightWithDeparture(flight);

            if (DateTime.Now! <= flight.ScheduledFlights.Last().ScheduledArrivalTime)
            {
                var t = DateTime.Now - flight.ScheduledFlights.Last().ScheduledArrivalTime;
                await Task.Delay(t.Duration());
            }
            

            while (flight.Airplane.CanTakeOff == false)
            {
                var checkTerminal = _terminalRepository.Get(flight.Terminal.Id);
                if (checkTerminal.IsOccupied == false)
                {
                    flight.Airplane.CanTakeOff = true;
                    flight.Terminal.IsOccupied = true;
                    await _flightRepository.Update(flight);
                }
            }
            flight.Airplane.CanTakeOff = false;
        }

        private async Task MakeFlight(Flight flight)
        {   
            var flightDTO = _mapper.Map<FlightDTO>(flight);
            await _hubContext.Clients.All.SendAsync("ReceiveFlight", flightDTO);

            UpdateWithArraival(flight);
            await Task.Delay(flight.Terminal.DurationInTerminal * 1000);
            await UpdateWithDeparture(flight);
        }

        private void UpdateWithArraival(Flight flight)
        {
            var fh = new FlightsHistory 
            {
                FlightId = flight.Id,
                ActualArrivalTime = DateTime.Now,
                //ActualDepartureTime = DateTime.Now.AddSeconds(flight.Terminal.DurationInTerminal),
            };

            flight.FlightsHistories.Add(fh);
            flight.Airplane.PreviousTerminal = flight.Airplane.CurrentTerminal;
            flight.Airplane.CurrentTerminal = flight.Terminal.Number;
        }

        //private async Task UpdatePreFlightWithDeparture(Flight flight)
        //{
        //    var preFlight = _flightRepository.GetAll().Where(f => f.AirplaneId == flight.AirplaneId).TakeLast(2).First();

        //    if (DateTime.Now !<= preFlight.FlightsHistories.Last().ActualDepartureTime)
        //    {
        //        var t = DateTime.Now - preFlight.FlightsHistories.Last().ActualDepartureTime;
        //        await Task.Delay(t.Duration());
        //    }
        //    preFlight.FlightsHistories.Last().ActualDepartureTime = DateTime.Now;
        //    preFlight.Terminal.IsOccupied = false;
        //    await _flightRepository.Update(preFlight);
        //}

        private async Task UpdateWithDeparture(Flight flight)
        {
            flight.FlightsHistories.Last().ActualDepartureTime = DateTime.Now;
            flight.Terminal.IsOccupied = false;
            await _flightRepository.Update(flight);
        }

        private Terminal SetTerminal(Airplane airplane)
        {
            int terminalId = 0;

            if (airplane.CurrentTerminal == null)
                terminalId = 1;
            else if (airplane.CurrentTerminal == 5)
            {
                while (terminalId == 0)
                {
                    var checkTerminal = _terminalRepository.GetAll().Where(t => t.Id > 5 && t.Id < 8).ToArray();
                    if (!checkTerminal[0].IsOccupied)
                        terminalId = 6;
                    else if (!checkTerminal[1].IsOccupied)
                        terminalId = 7;
                }
            }
            else if (airplane.CurrentTerminal == 6)
                terminalId = 8;
            else if(airplane.CurrentTerminal == 8)
                terminalId = 4;
            else if(airplane.CurrentTerminal == 4 && airplane.PreviousTerminal == 8)
                terminalId = 9;
            else
              terminalId = (int)airplane.CurrentTerminal + 1;

            var terminal = _terminalRepository.Get(terminalId);
            SetDurationInTerminal(terminal);
            return terminal;
        }

        private void SetDurationInTerminal(Terminal terminal)
        {
            Random random = new();

            if (terminal.Id == 6 || terminal.Id == 7)
                terminal.DurationInTerminal = random.Next(6,10);
            else if (terminal.Id == 4 || terminal.Id == 9)
                terminal.DurationInTerminal = random.Next(2, 4);
            else
                terminal.DurationInTerminal = random.Next(3, 6);
        }

        private DateTime SetScheduledArrivalTime(Flight flight)
        {
            if (flight.Airplane.CurrentTerminal == null)
                return DateTime.Now;
            else
            {
                var terminal = _terminalRepository.Get((int)flight.Airplane.CurrentTerminal);
                return DateTime.Now.AddSeconds(terminal.DurationInTerminal);
            }

        }

        private DateTime SetScheduledDepartureTime(Flight flight)
        {
            return DateTime.Now.AddSeconds(flight.Terminal.DurationInTerminal);
        }
    }
}
