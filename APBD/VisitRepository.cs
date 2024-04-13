using System.Collections.Generic;

namespace AppConsole1
{
    public class VisitRepository
    {
        private List<Visit> visits;

        public VisitRepository()
        {
            visits = new List<Visit>();
        }

        // Method to retrieve a list of visits associated with a given animal
        public List<Visit> RetrieveVisitsByAnimal(ConsoleApp1.Animal animal)
        {
            return visits.FindAll(v => v.Animal.id == animal.id);
        }

        // Method to add a new visit
        public void AddVisit(Visit visit)
        {
            visits.Add(visit);
        }
    }
}