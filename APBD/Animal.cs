namespace ConsoleApp1
{
    public class Animal
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string weight {  get; set; }
        public string furColor { get; set; }

        public Animal(int id, string name, string category, string weight, string furColor)
        {
            id = id;
            name = name;
            category = category;
            weight = weight;
            furColor = furColor;
        }
    }
}