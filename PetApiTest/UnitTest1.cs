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
            var response = await client.GetAsync($"petStore/GetOnePet/{name}");
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
            var response = await client.DeleteAsync($"petStore/DeleteOnePet/{name}");
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);

            //then
            response.EnsureSuccessStatusCode();
            Assert.Equal(new List<Pet>() { pet1 }, actualPets);
        }
    }
}
