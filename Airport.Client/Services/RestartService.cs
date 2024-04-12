using Airport.Data.Models;
using Airport.Data.Repository;

namespace Airport.Client.Services
{
    public class RestartService : IHostedService
    {
        private readonly IRepository<Airplane> _airplaneRepository;
        private readonly IRepository<Terminal> _terminalRepository;
        private readonly FlightService _flightService;

        public RestartService(IRepository<Airplane> airportRepository, IRepository<Terminal> terminalRepository, FlightService flightService) 
        {
            _airplaneRepository = airportRepository;
            _terminalRepository = terminalRepository;
            _flightService = flightService;
        }


        public async Task RestartAirport()
        {
            await RestartTerminals();
            await RestartAirplanes();

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RestartAirport();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task RestartAirplanes()
        {
            var airplanes = _airplaneRepository.GetAll()
                                                           .Where(a => a.CurrentTerminal >= 1 && a.CurrentTerminal < 9)
                                                           .TakeLast(10)
                                                           .ToList();
            foreach (var airplane in airplanes)
                await _flightService.StartAirportSimulation(airplane);
        }

        private async Task RestartTerminals()
        {
            var terminals = _terminalRepository.GetAll()
                                               .Where(t => t.IsOccupied)
                                               .ToList();

            foreach (var terminal in terminals)
            {
                terminal.IsOccupied = false;
                await _terminalRepository.Update(terminal);
            }
        }
    }
}
