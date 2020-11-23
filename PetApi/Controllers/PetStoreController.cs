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
        //[Route("FindPetByItsName/")]
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

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
