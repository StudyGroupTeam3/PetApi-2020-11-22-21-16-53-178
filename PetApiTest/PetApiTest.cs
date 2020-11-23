using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using PetApi.Controllers;
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

        // petStore/Pets/{name}
        [Fact]
        public async void Should_Return_Correct_Pets_When_Get_All_Pets()
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

            Assert.Equal(new List<Pet>() { pet }, actualPet);
        }

        // petStore/Pets/{name}
        [Fact]
        public async void Should_Get_Pet_Off_When_Buy_Pet_By_Name()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            await client.DeleteAsync("petStore/clear");

            Pet pet = new Pet("Tom", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            Pet pet1 = new Pet("Jerry", "cat", "white", 5000);
            string request1 = JsonConvert.SerializeObject(pet1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody1);

            // when
            await client.DeleteAsync($"petStore/Pets/{pet.Name}");
            var response = await client.GetAsync("petStore/Pets");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(new List<Pet>() { pet1 }, actualPet);
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

        // petStore/Pets/type/{type}
        [Fact]
        public async void Should_Return_Correct_Pets_When_Get_Pet_By_Type()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            await client.DeleteAsync("petStore/clear");

            Pet pet1 = new Pet("Tom", "dog", "white", 5000);
            string request1 = JsonConvert.SerializeObject(pet1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody1);

            Pet pet2 = new Pet("Tom2", "dog", "white", 5000);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody2);

            Pet pet3 = new Pet("Tom3", "cat", "white", 5000);
            string request3 = JsonConvert.SerializeObject(pet3);
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody3);

            // when
            var response = await client.GetAsync($"petStore/Pets/type/dog");

            // then
            response.EnsureSuccessStatusCode();
            var expected = new List<Pet>() { new Pet("Tom", "dog", "white", 5000), new Pet("Tom2", "dog", "white", 5000) };
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(expected, actualPet);
        }

        // petStore/Pets/color/{color}
        [Fact]
        public async void Should_Return_Correct_Pets_When_Get_Pet_By_Color()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            await client.DeleteAsync("petStore/clear");

            Pet pet1 = new Pet("Tom", "dog", "white", 5000);
            string request1 = JsonConvert.SerializeObject(pet1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody1);

            Pet pet2 = new Pet("Tom2", "dog", "black", 5000);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody2);

            Pet pet3 = new Pet("Tom3", "cat", "black", 5000);
            string request3 = JsonConvert.SerializeObject(pet3);
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody3);

            // when
            var response = await client.GetAsync($"petStore/Pets/color/black");

            // then
            response.EnsureSuccessStatusCode();
            var expected = new List<Pet>() { new Pet("Tom2", "dog", "black", 5000), new Pet("Tom3", "cat", "black", 5000) };
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(expected, actualPet);
        }

        // petStore/Pets/price/{priceRange}
        [Fact]
        public async void Should_Return_Correct_Pets_When_Get_Pet_In_Price_Range()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            await client.DeleteAsync("petStore/clear");

            Pet pet1 = new Pet("Tom", "dog", "white", 100);
            string request1 = JsonConvert.SerializeObject(pet1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody1);

            Pet pet2 = new Pet("Tom2", "dog", "black", 20);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody2);

            Pet pet3 = new Pet("Tom3", "cat", "black", 5000);
            string request3 = JsonConvert.SerializeObject(pet3);
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody3);

            // when
            var priceRange = "0-1000";
            var response = await client.GetAsync($"petStore/Pets/price/{priceRange}");

            // then
            response.EnsureSuccessStatusCode();
            var expected = new List<Pet>() { new Pet("Tom", "dog", "white", 100), new Pet("Tom2", "dog", "black", 20) };
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            Assert.Equal(expected, actualPet);
        }

        // petStore/modifyPetPrice
        [Fact]
        public async void Should_Modify_Pet_Price_When_Modify_Pet_Price()
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
            Pet pet2 = new Pet("Bavmax", "dog", "white", 50);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PutAsync("petStore/modifyPetPrice", requestBody2);

            var response = await client.GetAsync($"petStore/Pets/Bavmax");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            Assert.Equal(50, actualPet.Price);
        }
    }
}
