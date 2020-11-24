using Microsoft.AspNetCore.Mvc;
using PetApi.Models;
using PetApiTest;
using System.Collections.Generic;
using System.Linq;

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
        public IList<Pet> GetPetsByProperty(string color, string type, int? minPrice, int? maxPrice)
        {
            return color == null && type == null && minPrice == null && maxPrice == null
                ? pets : pets.Where(pet => pet.Color == color || pet.Type == type || (pet.Price >= minPrice && pet.Price <= maxPrice)).ToList();
        }

        [HttpGet("Pets/{name}")]
        public Pet GetPetByName(string name)
        {
            return pets.FirstOrDefault(pet => pet.Name == name);
        }

        [HttpDelete]
        public void BuyPetByName(string name)
        {
            pets.Remove(pets.FirstOrDefault(pet => pet.Name == name));
        }

        [HttpPatch]
        public Pet ModifyPriceByName2(UpdateModel updateModel)
        {
            var petFound = pets.FirstOrDefault(pet => pet.Name == updateModel.Name);
            petFound.Price = updateModel.Price;

            return petFound;
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
