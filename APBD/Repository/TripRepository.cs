using Microsoft.EntityFrameworkCore;
using APBD.Context;
using APBD.Models;

namespace APBD.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly TripContext _context;

        public TripRepository(TripContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetTrips(int page, int pageSize)
        {
            return await _context.Trips
                                 .OrderByDescending(t => t.DateFrom)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .Include(t => t.CountryTrips)
                                 .ThenInclude(ct => ct.Country)
                                 .Include(t => t.ClientTrips)
                                 .ThenInclude(ct => ct.Client)
                                 .ToListAsync();
        }

        public async Task<int> GetTripsCount()
        {
            return await _context.Trips.CountAsync();
        }

        public async Task<Client> GetClientById(int idClient)
        {
            return await _context.Clients.Include(c => c.ClientTrips).FirstOrDefaultAsync(c => c.IdClient == idClient);
        }

        public async Task<bool> DeleteClient(int idClient)
        {
            var client = await _context.Clients.FindAsync(idClient);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignClientToTrip(Client client, int idTrip)
        {
            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null || trip.DateFrom <= DateTime.Now) return false;

            var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == client.Pesel);
            if (existingClient != null)
            {
                var clientTrip = await _context.ClientTrips.FirstOrDefaultAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);
                if (clientTrip != null) return false;

                client = existingClient;
            }
            else
            {
                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
            }

            var newClientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = client.ClientTrips.FirstOrDefault()?.PaymentDate
            };

            await _context.ClientTrips.AddAsync(newClientTrip);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}