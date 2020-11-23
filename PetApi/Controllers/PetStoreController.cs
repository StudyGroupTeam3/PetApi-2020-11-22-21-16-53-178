using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetApi.Models;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : ControllerBase
    {
        // static is important
        private static IList<Pet> pets = new List<Pet>();

        [HttpPost("addNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("Pets")]
        public IList<Pet> GetPets()
        {
            return pets;
        }

        [HttpGet("Pets/{name}")]
        public Pet GetPetByName(string name)
        {
            return pets.FirstOrDefault(pet => pet.Name == name);
        }

        [HttpGet("Pets/type/{type}")]
        public List<Pet> GetPetByType(string type)
        {
            return pets.Where(pet => pet.Type == type).ToList();
        }

        [HttpGet("Pets/color/{color}")]
        public List<Pet> GetPetByColor(string color)
        {
            return pets.Where(pet => pet.Color == color).ToList();
        }

        [HttpGet("Pets/price/{priceRange}")]
        public List<Pet> GetPetInPriceRange(string priceRange)
        {
            var startPrice = priceRange.Split('-')[0];
            var endPrice = priceRange.Split('-')[1];
            return pets.Where(pet => pet.Price >= int.Parse(startPrice) && pet.Price <= int.Parse(endPrice)).ToList();
        }

        [HttpDelete("Pets/{name}")]
        public void BuyPetByName(string name)
        {
            pets.Remove(pets.FirstOrDefault(pet => pet.Name == name));
        }

        [HttpPut("modifyPetPrice")]
        public Pet ModifyPriceByName(Pet newPet)
        {
            var petFound = pets.FirstOrDefault(pet => pet.Name == newPet.Name);
            petFound.Price = newPet.Price;

            return petFound;
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
