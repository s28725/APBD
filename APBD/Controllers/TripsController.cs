using APBD.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace APBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripRepository _repository;

        public TripsController(ITripRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var trips = await _repository.GetTrips(page, pageSize);
            var totalTrips = await _repository.GetTripsCount();
            var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

            var response = new
            {
                pageNum = page,
                pageSize,
                allPages = totalPages,
                trips = trips.Select(t => new
                {
                    t.Name,
                    t.Description,
                    t.DateFrom,
                    t.DateTo,
                    t.MaxPeople,
                    Countries = t.CountryTrips.Select(ct => new { ct.Country.Name }),
                    Clients = t.ClientTrips.Select(ct => new { ct.Client.FirstName, ct.Client.LastName })
                })
            };

            return Ok(response);
        }
    }
}