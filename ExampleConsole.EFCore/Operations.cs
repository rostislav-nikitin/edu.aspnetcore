
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExampleConsole.EFCore
{
    public class Operations
    {
        public async Task<Station> AddStation(Station station)
        {
            using(var context = new Context())
            {
                var entry = await context.AddAsync(station);

                context.SaveChanges();

                return entry.Entity;
            }
        }

        public async Task<Station> GetStation(int id)
        {
            using(var context = new Context())
            {
                var result = await context.Stations.FirstOrDefaultAsync(p => p.StationId == id);

                return result;
            }
        }
    }
}