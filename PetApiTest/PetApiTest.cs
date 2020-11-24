using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using PetApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        private readonly TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        private readonly HttpClient client;
        public PetApiTest()
        {
            client = server.CreateClient();
            client.DeleteAsync("petStore/clear");
        }

        // petStore/addNewPet
        [Fact]
        public async void AC1_Should_Add_Pet_When_Add_Pet()
        {
            // given
            var pet = new Pet("Bavmax", "dog", "white", 5000);
            var request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("petStore/addNewPet", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            Assert.Equal(pet, actualPet);
        }

        // petStore/Pets
        [Fact]
        public async void AC2_Should_Return_Correct_Pets_When_Get_All_Pets()
        {
            // given
            var pets = await AddPets();

            // when
            var response = await client.GetAsync("petStore/Pets");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(pets, actualPets);
        }

        // petStore/Pets/{name}
        [Fact]
        public async void AC3_Should_Return_Correct_Pet_When_Get_Pet_By_Name()
        {
            // given
            var pets = await AddPets();

            // when
            var response = await client.GetAsync($"petStore/Pets/{pets[0].Name}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            Assert.Equal(pets[0], actualPet);
        }

        // petStore/Pets/{name}
        [Fact]
        public async void AC4_Should_Get_Pet_Off_When_Buy_Pet_By_Name()
        {
            // given
            var pets = await AddPets();

            // when
            await client.DeleteAsync($"petStore/Pets/{pets[0].Name}");
            pets.Remove(pets[0]);
            var response = await client.GetAsync("petStore/Pets");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(pets, actualPets);
        }

        // petStore/modifyPetPrice
        [Fact]
        public async void AC_5_Should_Modify_Pet_Price_When_Modify_Pet_Price()
        {
            // given
            var pet = new Pet("Bavmax", "dog", "white", 5000);
            var request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            // when
            var newPet = new Pet("Bavmax", "dog", "white", 50);
            var newRequest = JsonConvert.SerializeObject(newPet);
            var newRequestBody = new StringContent(newRequest, Encoding.UTF8, "application/json");
            await client.PutAsync("petStore/modifyPetPrice", newRequestBody);
            var response = await client.GetAsync($"petStore/Pets/Bavmax");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            Assert.Equal(newPet.Price, actualPet.Price);
        }

        // petStore/Pets/type/{type}
        [Fact]
        public async void AC_6_Should_Return_Correct_Pets_When_Get_Pet_By_Type()
        {
            // given
            var pets = await AddPets();

            // when
            const string type = "dog";
            var response = await client.GetAsync($"petStore/Pets/type/{type}");

            // then
            response.EnsureSuccessStatusCode();
            var expectedPets = pets.Where(pet => pet.Type == "dog").ToList();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(expectedPets, actualPets);
        }

        // petStore/Pets/price/{priceRange}
        [Fact]
        public async void AC7_Should_Return_Correct_Pets_When_Get_Pet_In_Price_Range()
        {
            // given
            var pets = await AddPets();

            // when
            const string priceRange = "0-1000";
            var response = await client.GetAsync($"petStore/Pets/price/{priceRange}");

            // then
            response.EnsureSuccessStatusCode();
            var expectedPets = pets.Where(pet => pet.Price >= 0 && pet.Price <= 1000).ToList();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(expectedPets, actualPets);
        }

        // petStore/Pets/color/{color}
        [Fact]
        public async void AC8_Should_Return_Correct_Pets_When_Get_Pet_By_Color()
        {
            // given
            var pets = await AddPets();

            // when
            const string color = "white";
            var response = await client.GetAsync($"petStore/Pets/color/{color}");

            // then
            response.EnsureSuccessStatusCode();
            var expectedPets = pets.Where(pet => pet.Color == color).ToList();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(expectedPets, actualPets);
        }

        // petStore
        [Fact]
        public async void Patch_Should_Modify_Pet_Price_When_Modify_Pet_Price()
        {
            // given
            await AddPets();

            // when
            var updateData = new UpdateModel("Tom", 10);
            var request = JsonConvert.SerializeObject(updateData);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync("petStore", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            Assert.Equal(updateData.Price, actualPet.Price);
        }

        private async Task<List<Pet>> AddPets()
        {
            var pets = new List<Pet>()
            {
                new Pet("Tom", "dog", "white", 5000),
                new Pet("Tom2", "dog", "black", 500),
                new Pet("Tom3", "cat", "white", 50),
            };

            foreach (var requestBody in pets.Select(JsonConvert.SerializeObject)
                .Select(request => new StringContent(request, Encoding.UTF8, "application/json")))
            {
                await client.PostAsync("petStore/addNewPet", requestBody);
            }

            return pets;
        }
    }
}
