using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using PetApi.Models;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        // petStore/addNewPet
        [Fact]
        public async void Should_Add_Pet_When_Add_Pet()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Pet pet = new Pet("Bavmax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("petStore/addNewPet", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            Assert.Equal(pet, actualPet);
        }

        // petStore/Pets
        [Fact]
        public async void Should_Return_Correct_Pets_When_Get_All_Pet()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            await client.DeleteAsync("petStore/clear");

            Pet pet = new Pet("Bavmax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            // when
            var response = await client.GetAsync("petStore/Pets");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(new List<Pet>() { new Pet("Bavmax", "dog", "white", 5000) }, actualPet);
        }

        // petStore/Pets/{name}
        [Fact]
        public async void Should_Return_Correct_Pet_When_Get_Pet_By_Name()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            await client.DeleteAsync("petStore/clear");

            Pet pet = new Pet("Tom", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            // when
            var response = await client.GetAsync($"petStore/Pets/{pet.Name}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            Assert.Equal(pet, actualPet);
        }
    }
}
