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

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }

        [HttpGet("{name:alpha}")]
        public Pet GetByName(string name)
        {
            return pets.FirstOrDefault(pet => pet.Name == name);
        }

        [HttpDelete("{name:alpha}")]
        public void GetOffShelf(string name)
        {
            pets.RemoveAt(pets.Where((pet, index) => pet.Name == name).Select((pet, index) => new { pet, index }).ToList().First().index);
        }

        [HttpPatch("{name:alpha}")]
        public void ModifyPetPrice(string name, PetPriceModifyModel petPriceModifyModel)
        {
            pets.FirstOrDefault(pet => pet.Name == name).Price = petPriceModifyModel.Price;
        }

        [HttpGet]
        public List<Pet> GetByProperty(string type)
        {
            return pets.Where(pet => pet.Type == type).ToList();
        }
    }

    public class PetPriceModifyModel
    {
        public PetPriceModifyModel()
        {
        }

        public PetPriceModifyModel(double price)
        {
            this.Price = price;
        }

        public double Price { get; set; }
    }
}
