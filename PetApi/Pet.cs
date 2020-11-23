namespace PetApi
{
    public class Pet
    {
        public Pet()
        {
        }

        public Pet(string v1, string v2, string v3, int v4)
        {
            this.Name = v1;
            this.Type = v2;
            this.Color = v3;
            this.Price = v4;
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

            Pet otherPet = (Pet)obj;
            return Name == otherPet.Name && Type == otherPet.Type && Color == otherPet.Color && Price == otherPet.Price;
        }
    }
}