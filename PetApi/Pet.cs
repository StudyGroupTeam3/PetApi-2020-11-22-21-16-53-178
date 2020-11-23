namespace PetApiTest
{
    public class Pet
    {
        public Pet()
        {
        }

        public Pet(string name, string type, string color, int price)
        {
            Name = name;
            Type = type;
            Color = color;
            Price = price;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
        public override bool Equals(object? obj)
        {
            if (!(obj is Pet))
            {
                return false;
            }

            var pet1 = (Pet)obj;
            return Compare(pet1);
        }

        protected bool Compare(Pet pet1)
        {
            return pet1.Name == Name && pet1.Type == Type && pet1.Color == Color &&
                   pet1.Price == Price;
        }
    }
}