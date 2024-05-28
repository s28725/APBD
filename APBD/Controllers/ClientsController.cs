using Microsoft.AspNetCore.Mvc;
using APBD.Models;
using APBD.Repositories;

namespace APBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ITripRepository _repository;

        public ClientsController(ITripRepository repository)
        {
            _repository = repository;
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> RemoveClient(int clientId)
        {
            var existingClient = await _repository.GetClientById(clientId);
            if (existingClient == null)
                return NotFound(new { message = "The specified client does not exist." });

            if (existingClient.ClientTrips.Any())
                return BadRequest(new { message = "The client cannot be deleted because they have trips assigned." });

            var deleteResult = await _repository.DeleteClient(clientId);
            if (!deleteResult)
                return StatusCode(500, new { message = "An error occurred while trying to delete the client." });

            return NoContent();
        }

        [HttpPost("{tripId}/clients")]
        public async Task<IActionResult> AddClientToTrip(int tripId, [FromBody] Client newClient)
        {
            var assignmentResult = await _repository.AssignClientToTrip(newClient, tripId);
            if (!assignmentResult)
                return BadRequest(new { message = "Failed to assign the client to the trip." });

            return Ok(new { message = "The client was successfully assigned to the trip." });
        }
    }
}