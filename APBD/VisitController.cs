using AppConsole1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ConsoleApp1
{
    [Route("api/visit")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        private readonly VisitRepository visitRepository;
        private readonly AnimalRepository animalRepository;

        public VisitController(VisitRepository visitRepository, AnimalRepository animalRepository)
        {
            this.visitRepository = visitRepository;
            this.animalRepository = animalRepository;
        }


        [HttpPost]
        public IActionResult AddVisit([FromBody] Visit visit)
        {
            if (visit == null)
            {
                return BadRequest();
            }

            visitRepository.AddVisit(visit);

            return Ok("Visit created successfully");
        }

        [HttpGet("animal/{animalId}")]
        public ActionResult<IEnumerable<Visit>> GetVisitsByAnimal(int animalId)
        {
            var animal = animalRepository.RetrieveAnimalById(animalId);
            if (animal == null)
            {
                return NotFound("Animal not found");
            }

            var visits = visitRepository.RetrieveVisitsByAnimal(animal);
            return Ok(visits);
        }
    }
}
