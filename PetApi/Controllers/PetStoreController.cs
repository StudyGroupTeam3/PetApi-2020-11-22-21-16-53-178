using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        [HttpGet]
        [Route("{name?}")]
        public Pet GetPetByItsName(string name)
        {
            return pets.FirstOrDefault(x => x.Name == name);
        }

        [HttpDelete]
        [Route("{name}")]
        public List<Pet> DeleteBoughtPet(string name)
        {
            pets.RemoveAll(x => x.Name == name);
            return pets;
        }

        [HttpPatch]
        [Route("ModifyPetPrice")]
        public Pet ModifyPrice(Pet pet)
        {
            var modifiedPet = pets.FirstOrDefault(x => x.Name == pet.Name);
            modifiedPet.Price = pet.Price;
            return modifiedPet;
        }

        [HttpGet]
        [Route("FindPetByItsType/")]
        public List<Pet> GetPetByItsType(string type)
        {
            return pets.Where(x => x.Type == type).ToList();
        }

        [HttpGet]
        [Route("FindPetByPriceRange/")]
        public List<Pet> GetPetByPriceRange(int min, int max)
        {
            return pets.Where(x => x.Price >= min && x.Price <= max).ToList();
        }

        [HttpGet]
        [Route("FindPetByItsColor/")]
        public List<Pet> GetPetByItsColor(string color)
        {
            return pets.Where(x => x.Color == color).ToList();
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
