using ConsoleApp1;
using System;

namespace AppConsole1
{
    public class Visit
    {
        public DateTime Date { get; set; }
        public Animal Animal { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Visit(DateTime date, Animal animal, string description, decimal price)
        {
            Date = date;
            Animal = animal;
            Description = description;
            Price = price;
        }
    }
}