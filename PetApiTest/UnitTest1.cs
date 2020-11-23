using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        //petStore/addNewPet
        [Fact]
        public async Task Should_Return_Correct_Pets_When_Get_All_Pets()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 3000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            //when
            var response = await client.PostAsync("petStore/addNewPet", requestBody);

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_Add_Pet_When_Add_Pet_()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet(name: "Hello", type: "cat", color: "white", price: 3000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);

            //when
            var response = await client.GetAsync("petStore/pets");
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }

        [Fact]
        public async Task Should_FindPet_Return_Correct_Pet_With_Given_Name()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet(name: "Hello", type: "cat", color: "white", price: 3000);
            Pet pet1 = new Pet(name: "Kitty", type: "cat", color: "black", price: 3000);
            var name = pet.Name;
            string request = JsonConvert.SerializeObject(pet);
            string request1 = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent requestBody1 = new StringContent(request, Encoding.UTF8, "application/json");
            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);
            await client.PostAsync("petStore/addNewPet", requestBody1);

            //when
            var response = await client.GetAsync($"petStore/getOnePet/{name}");
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_DeleteByPetName_Succeed()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet(name: "Hello", type: "cat", color: "white", price: 3000);
            Pet pet1 = new Pet(name: "Kitty", type: "cat", color: "black", price: 3000);
            var name = pet.Name;
            string request = JsonConvert.SerializeObject(pet);
            string request1 = JsonConvert.SerializeObject(pet1);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);
            await client.PostAsync("petStore/addNewPet", requestBody1);

            //when
            var response = await client.DeleteAsync($"petStore/deleteOnePet/{name}");
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(new List<Pet>() { pet1 }, actualPets);
        }

        [Fact]
        public async Task Should_ModifyPrice_Return_Right_Pet_With_Modifed_Price()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet(name: "Hello", type: "cat", color: "white", price: 3000);
            Pet pet1 = new Pet(name: "Kitty", type: "cat", color: "black", price: 3000);
            string request = JsonConvert.SerializeObject(pet);
            string request1 = JsonConvert.SerializeObject(pet1);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);
            await client.PostAsync("petStore/addNewPet", requestBody1);
            UpdateModel updateInfo = new UpdateModel(name: "Hello", price: 5000);
            string request2 = JsonConvert.SerializeObject(updateInfo);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            //when
            var response = await client.PatchAsync($"petStore/newPriceInfo", requestBody2);
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(new Pet(name: "Hello", type: "cat", color: "white", price: 5000), actualPet);
        }

        [Fact]
        public async Task Should_FindByType_Return_Right_Pet_List()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet(name: "Hello", type: "cat", color: "white", price: 3000);
            Pet pet1 = new Pet(name: "Kitty", type: "cat", color: "black", price: 3000);
            Pet pet2 = new Pet(name: "Lightning", type: "dog", color: "yellow", price: 4000);
            string request = JsonConvert.SerializeObject(pet);
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);
            await client.PostAsync("petStore/addNewPet", requestBody1);
            await client.PostAsync("petStore/addNewPet", requestBody2);
            string type = "cat";

            //when
            var response = await client.GetAsync($"petStore/getByType/{type}");
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(new List<Pet>() { pet, pet1 }, actualPets);
        }
    }
}
