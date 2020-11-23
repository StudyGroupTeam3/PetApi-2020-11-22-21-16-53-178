using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : ControllerBase
    {
        private static List<Pet> pets = new List<Pet>();

        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("pets")]
        public IList<Pet> GetAllPet()
        {
            return pets;
        }

        [HttpGet("getPetByName/{petName}")]
        public Pet GetPetByName(string petName)
        {
            IEnumerable<Pet> foundPets =
                from pet in pets
                where pet.Name == petName
                select pet;
            return foundPets.ToList().Count == 0 ? null : foundPets.ToList()[0];
        }

        [HttpDelete("deletePetByName/{petName}")]
        public void DeletePetByName(string petName)
        {
            var deletePet = pets.Find(pet => pet.Name == petName);
            pets.Remove(deletePet);
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
