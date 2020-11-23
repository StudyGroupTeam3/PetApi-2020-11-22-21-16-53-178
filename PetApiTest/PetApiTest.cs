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
    }
}
