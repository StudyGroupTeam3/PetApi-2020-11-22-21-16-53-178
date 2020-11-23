using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : ControllerBase
    {
        private static IList<Pet> pets = new List<Pet>();

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
            return foundPets.ToList()[0];
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
