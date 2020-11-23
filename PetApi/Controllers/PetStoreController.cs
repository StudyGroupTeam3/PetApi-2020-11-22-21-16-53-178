using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

        [HttpPut("modifyPet/{petName}")]
        public void ModifyPet(string petName, Pet newPet)
        {
            var index = pets.FindIndex(pet => pet.Name == petName);
            pets[index] = newPet;
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

        [HttpGet("getPetsByType/{petType}")]
        public IList<Pet> GetPetsByType(string petType)
        {
            IEnumerable<Pet> foundPets =
                from pet in pets
                where pet.Type == petType
                select pet;
            return foundPets.ToList().Count == 0 ? null : foundPets.ToList();
        }

        [HttpGet("getPetsByColor/{petColor}")]
        public IList<Pet> GetPetsByColor(string petColor)
        {
            IEnumerable<Pet> foundPets =
                from pet in pets
                where pet.Color == petColor
                select pet;
            return foundPets.ToList().Count == 0 ? null : foundPets.ToList();
        }

        [HttpGet("getPetsByPriceRange/{priceRange}")]
        public IList<Pet> GetPetsByPriceRange(string priceRange)
        {
            double.TryParse(priceRange.Split("-")[0], out double startRange);
            double.TryParse(priceRange.Split("-")[1], out double endRange);
            IEnumerable<Pet> foundPets =
                from pet in pets
                where pet.Price > startRange && pet.Price < endRange
                select pet;
            return foundPets.ToList().Count == 0 ? null : foundPets.ToList();
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
