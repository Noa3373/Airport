using Airport.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Repository
{
    public class TerminalRepository : IRepository<Terminal>
    {
        private readonly AirportDbContext _context;

        public TerminalRepository(AirportDbContext context) => _context = context;
        public async Task<int> Add(Terminal terminal)
        {
            var entry = _context.Add(terminal);
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }

        public void Delete(int terminalNumber)
        {
            var terminal = Get(terminalNumber);
            if (terminal != null)
            {
                _context.Remove(terminal);
                _context.SaveChanges();
            }
        }

        public Terminal? Get(int terminalNumber) => GetAll().FirstOrDefault(t => t.Number == terminalNumber);

        public IEnumerable<Terminal> GetAll() => _context.Terminals;

        public async Task<int> Update(Terminal terminal)
        {
            var entry = _context.Update(terminal);
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }

        public async Task DeleteAll()
        {
            var terminals = await _context.Terminals.ToListAsync();
            _context.Terminals.RemoveRange(terminals);
            await _context.SaveChangesAsync();
        }
    }
}
