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

        [HttpDelete("Pets/{name}")]
        public void BuyPetByName(string name)
        {
            pets.Remove(pets.FirstOrDefault(pet => pet.Name == name));
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
