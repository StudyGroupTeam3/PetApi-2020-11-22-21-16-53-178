using System;
using System.Collections.Generic;
using System.Text;

namespace PetApiTest
{
    public class UpdateModel
    {
        public UpdateModel()
        {
        }

        public UpdateModel(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }
        public int Price { get; set; }
    }
}
