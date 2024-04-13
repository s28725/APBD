using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ConsoleApp1
{
    [Route("api/animal")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly AnimalRepository animalRepository;

        public AnimalController(AnimalRepository animalRepository)
        {
            animalRepository = animalRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAnimals()
        {
            return Ok(animalRepository.RetrieveAnimals());
        }

        [HttpGet("{id}")]
        public ActionResult<Animal> GetAnimal(int id)
        {
            Animal animal = animalRepository.RetrieveAnimalById(id);
            if (animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }

        [HttpPost]
        public IActionResult AddAnimal(Animal animal)
        {
            animalRepository.AddAnimal(animal);
            return CreatedAtAction(nameof(GetAnimal), new { id = animal.id }, animal);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnimal(int id, Animal animal)
        {
            if (id != animal.id)
            {
                return BadRequest();
            }

            animalRepository.EditAnimal(id, animal);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnimal(int id)
        {
            var existingAnimal = animalRepository.RetrieveAnimalById(id);
            if (existingAnimal == null)
            {
                return NotFound();
            }

            animalRepository.DeleteAnimalById(id);
            return NoContent();
        }
    }
}