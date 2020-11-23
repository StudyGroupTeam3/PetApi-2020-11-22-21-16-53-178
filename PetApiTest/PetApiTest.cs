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
        [Fact]
        public async Task Should_Add_Pet_When_Add_Pet()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Pet pet = new Pet(name: "Baymas", type: "dog", color: "White", price: 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("PetStore/addNewPet", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet acutalPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, acutalPet);
        }

        [Fact]
        public async Task Should_Return_Correct_Pets_When_Get_All_Pets()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            // when
            Pet pet = new Pet(name: "Baymas", type: "dog", color: "White", price: 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody);

            var response = await client.GetAsync("PetStore/pets");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> acutalPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, acutalPet);
        }

        [Fact]
        public async Task Should_Find_Pet_When_Give_Pet_Name()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Pet pet1 = new Pet(name: "Baymas", type: "dog", color: "White", price: 5000);
            Pet pet2 = new Pet(name: "Apple", type: "cat", color: "White", price: 5000);
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody1);
            await client.PostAsync("PetStore/addNewPet", requestBody2);

            // when
            var response = await client.GetAsync("PetStore/Baymas");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet acutalPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet1,  acutalPet);
        }

        [Fact]
        public async Task Should_Delete_Pet_When_It_Is_Bought()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Pet pet1 = new Pet(name: "MiaoMiao", type: "cat", color: "Black", price: 1000);
            Pet pet2 = new Pet(name: "Orange", type: "dog", color: "White", price: 2000);
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody1);
            await client.PostAsync("PetStore/addNewPet", requestBody2);

            // when
            var response = await client.DeleteAsync("PetStore/Orange");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> acutalPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.False(acutalPet.Contains(pet2));
        }

        [Fact]
        public async Task Should_Modify_Pet_Price_When_It_is_Needed()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Pet pet1 = new Pet(name: "WangWang", type: "dog", color: "Black", price: 3000);
            string request1 = JsonConvert.SerializeObject(pet1);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody1);

            Pet pet2 = new Pet(name: "WangWang", type: "dog", color: "Black", price: 5000);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody2);

            // when
            var response = await client.PatchAsync("PetStore/ModifyPetPrice", requestBody2);
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet acutalPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.NotEqual(pet1.Price, acutalPet.Price);
        }

        [Fact]
        public async Task Should_Find_Pet_When_Give_Pet_Type()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Pet pet1 = new Pet(name: "Wang", type: "dog", color: "White", price: 5000);
            Pet pet2 = new Pet(name: "Miao", type: "cat", color: "White", price: 5000);
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody1);
            await client.PostAsync("PetStore/addNewPet", requestBody2);

            // when
            var response = await client.GetAsync("PetStore/FindPetByItsType?type=dog");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> acutalPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.True(acutalPet.Contains(pet1));
        }
    }
}
