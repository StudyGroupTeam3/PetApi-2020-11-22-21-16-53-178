using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetApiTest;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : ControllerBase
    {
        private static List<Pet> pets = new List<Pet>();

        [HttpPost("addNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("pets")]
        public IEnumerable<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpGet("getByType/{type}")]
        public List<Pet> GetPetsByType(string type)
        {
            return pets.Where(pet => pet.Type == type).ToList();
        }

        [HttpGet("getOnePet/{name}")]
        public Pet GetPetByName(string name)
        {
            return pets.Where(pet => pet.Name == name).ToList()[0];
        }

        [HttpGet("getByPriceRange/{priceRange}")]
        public List<Pet> GetPetsByPriceRange(string priceRange)
        {
            var input = priceRange.Split("-");
            int lowerPrice = int.Parse(input[0]);
            int upperPrice = int.Parse(input[1]);
            return pets.Where(pet => pet.Price >= lowerPrice && pet.Price <= upperPrice).ToList();
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }

        [HttpDelete("deleteOnePet/{name}")]
        public List<Pet> DeleteByName(string name)
        {
            Pet petToDelete = pets.Where(pet => pet.Name == name).ToList()[0];
            pets.Remove(petToDelete);
            return pets;
        }

        [HttpPatch("newPriceInfo")]
        public Pet UpdatePetPrice(UpdateModel update)
        {
            Pet result = new Pet();
            foreach (var pet in pets)
            {
                if (pet.Name == update.Name)
                {
                    pet.Price = update.Price;
                    result = pet;
                }
            }

            return result;
        }
    }
}
