using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using PetApi;
using PetApi.Controllers;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        [Fact]
        public async Task Should_Add_Pet_When_Add_Pet()
        {
            // given
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            Pet pet = new Pet("Baymax", "dog", "white", 5000);
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

        [Fact]
        public async Task Should_Return_All_Pets_When_Get_All_Pets()
        {
            // given
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            await client.DeleteAsync("petStore/clear");
            Pet pet = new Pet("Baymax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            // when
            var response = await client.GetAsync("petStore/pets");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }

        [Fact]
        public async Task Should_Return_Pet_With_Right_Name_When_Get_By_Name()
        {
            // given
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            await client.DeleteAsync("petStore/clear");
            Pet pet = new Pet("Baymax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            // when
            var response = await client.GetAsync("petStore/Baymax");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_Delete_A_Pet_When_Bought_By_A_Customer()
        {
            // given
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            await client.DeleteAsync("petStore/clear");
            Pet pet = new Pet("Baymax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            // when
            var response = await client.DeleteAsync("petStore/Baymax");
            response.EnsureSuccessStatusCode();
            // then
            var responseGetAllPets = await client.GetAsync("petStore/pets");
            responseGetAllPets.EnsureSuccessStatusCode();
            var responseString = await responseGetAllPets.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.True(actualPets.Count == 0);
        }

        [Fact]
        public async Task Should_Change_Price_Of_Pet_When_Modify_Pet_Price()
        {
            // given
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            await client.DeleteAsync("petStore/clear");
            Pet pet = new Pet("Baymax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            // when
            PetPriceModifyModel petPriceModifyModel = new PetPriceModifyModel(2000);
            string patchRequest = JsonConvert.SerializeObject(petPriceModifyModel);
            StringContent patchRequestBody = new StringContent(patchRequest, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync("petStore/Baymax", patchRequestBody);
            response.EnsureSuccessStatusCode();
            // then
            var responseGetPet = await client.GetAsync("petStore/Baymax");
            responseGetPet.EnsureSuccessStatusCode();
            var responseString = await responseGetPet.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.True(actualPet.Price == 2000);
        }
    }
}
