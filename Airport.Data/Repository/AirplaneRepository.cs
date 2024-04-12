using Airport.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Data.Repository
{
    public class AirplaneRepository : IRepository<Airplane>
    {
        private readonly AirportDbContext _context;

        public AirplaneRepository(AirportDbContext context) => _context = context;
        public async Task<int> Add(Airplane airplane)
        {
            var entry = _context.Add(airplane);
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }

        public void Delete(int id)
        {
            var airplane = Get(id);
            if (airplane != null)
            {
                _context.Remove(airplane);
                _context.SaveChanges();
            }
        }

        public Airplane? Get(int id) => GetAll().FirstOrDefault(a => a.Id == id);

        public IEnumerable<Airplane> GetAll() => _context.Airplanes;

        public async Task<int> Update(Airplane airplane)
        {
            var entry = _context.Update(airplane);
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }

        public async Task DeleteAll()
        {
            var airplanes = await _context.Airplanes.ToListAsync();
            _context.Airplanes.RemoveRange(airplanes);
            await _context.SaveChangesAsync();
        }
    }
}
