using Airport.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Repository
{
    public class FlightRepository : IRepository<Flight>
    {
        private readonly AirportDbContext _context;

        public FlightRepository(AirportDbContext context) => _context = context;


        public async Task<int> Add(Flight flight)
        {
            var flightEntry = _context.Add(flight);
            await _context.SaveChangesAsync();
            return flightEntry.Entity.Id;
        }

        public void Delete(int id)
        {
            var flight = Get(id);
            if (flight != null)
            {
                _context.Remove(flight);
                _context.SaveChanges();
            }
        }

        public Flight? Get(int id) => GetAll().FirstOrDefault(f => f.Id == id);

        public IEnumerable<Flight> GetAll() => _context.Flights
                                               .Include(f => f.Airplane)
                                               .Include(f => f.Terminal)
                                               .Include(f => f.ScheduledFlights)
                                               .Include(f => f.FlightsHistories);

        public async Task<int> Update(Flight flight)
        {
            var flightEntry = _context.Update(flight);
            await _context.SaveChangesAsync();
            return flightEntry.Entity.Id;
        }

        //זה בשבילי
        public async Task DeleteAll()
        {
            var scheduledFlights = await _context.ScheduledFlights.ToListAsync();
            _context.ScheduledFlights.RemoveRange(scheduledFlights);
            var flightsHistories = await _context.FlightsHistories.ToListAsync();
            _context.FlightsHistories.RemoveRange(flightsHistories);
            var flights = await _context.Flights.ToListAsync();
            _context.Flights.RemoveRange(flights);
            var airplanes = await _context.Airplanes.ToListAsync();
            _context.Airplanes.RemoveRange(airplanes);
            await _context.SaveChangesAsync();
        }
    }
}
