using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi
{
    public class Pet
    {
        public Pet()
        {
        }

        public Pet(string name, string type, string color, double price)
        {
            this.Name = name;
            this.Type = type;
            this.Color = color;
            this.Price = price;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Pet)obj);
        }

        private bool Equals(Pet other)
        {
            return Name == other.Name && Type == other.Type && Color == other.Color && Price == other.Price;
        }
    }
}
