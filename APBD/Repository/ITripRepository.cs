using APBD.Models;

namespace APBD.Repositories
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetTrips(int page, int pageSize);
        Task<int> GetTripsCount();
        Task<Client> GetClientById(int idClient);
        Task<bool> DeleteClient(int idClient);
        Task<bool> AssignClientToTrip(Client client, int idTrip);
    }
}