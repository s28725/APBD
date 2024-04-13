using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class AnimalRepository
    {
        private List<Animal> animals;

        public AnimalRepository()
        {
            animals = new List<Animal>()
            {
                new Animal(1, "Animal1", "Category1", "11", "Color1"),
                new Animal(2, "Animal2", "Category2", "12", "Color2"),
                new Animal(3, "Animal3", "Category3", "13", "Color3"),
                new Animal(4, "Animal4", "Category4", "14", "Color4"),
                new Animal(5, "Animal5", "Category5", "15", "Color5"),
                new Animal(6, "Animal6", "Category6", "16", "Color6"),
                new Animal(7, "Animal7", "Category7", "17", "Color7"),
                new Animal(8, "Animal8", "Category8", "18", "Color8"),
                new Animal(9, "Animal9", "Category9", "19", "Color9"),
                new Animal(10, "Animal10", "Category10", "20", "Color10")
            };
        }

        public List<Animal> RetrieveAnimals()
        {
            return animals;
        }

        // Method to retrieve a specific animal by id
        public Animal RetrieveAnimalById(int id)
        {
            return animals.Find(a => a.id == id);
        }

        // Method to add an animal
        public void AddAnimal(Animal animal)
        {
            animals.Add(animal);
        }

        // Method to edit an animal (animal will be identified by id)
        public void EditAnimal(int id, Animal updatedAnimal)
        {
            Animal animalToUpdate = animals.Find(a => a.id == id);
            if (animalToUpdate != null)
            {
                animalToUpdate.name = updatedAnimal.name;
                animalToUpdate.category = updatedAnimal.category;
                animalToUpdate.weight = updatedAnimal.weight;
                animalToUpdate.furColor = updatedAnimal.furColor;
            }
            else
            {
                Console.WriteLine("Animal not found!");
            }
        }

        // Method to delete an animal by id
        public void DeleteAnimalById(int id)
        {
            Animal animalToDelete = animals.Find(a => a.id == id);
            if (animalToDelete != null)
            {
                animals.Remove(animalToDelete);
            }
            else
            {
                Console.WriteLine("Animal not found!");
            }
        }

    }
}