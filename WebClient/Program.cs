using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using WebAPIModels.Models;

namespace WebClient
{
    public class Program
    {
      
        static async Task Main(string[] args)
        {
            Region region = new Region() { RegionId = 11111, RegionDescription = "13" };
            string jsonString = JsonSerializer.Serialize(region);
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient(clientHandler);


            using (var response = await client.GetAsync("http://localhost:5276/api/provider/"))
            {
                string res = await response.Content.ReadAsStringAsync();
                List <ProviderModel>? list = JsonSerializer.Deserialize<List<ProviderModel>>(res, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                Console.WriteLine(res);
                foreach(var i in list)
                {
                    Console.WriteLine(i.Name);
                }
            }

            using (var response = await client.PostAsync("http://localhost:5289/api/Region/", httpContent))
            {
                    string res = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(res);
            }

            using (var response = await client.PutAsync("http://localhost:5289/api/Region/1", httpContent))
            {
                string res = await response.Content.ReadAsStringAsync();
                Console.WriteLine(res);
            }

            using (var response = await client.DeleteAsync("http://localhost:5289/api/Region/108"))
            {
                string res = await response.Content.ReadAsStringAsync();
                Console.WriteLine(res);
            }

        }
    }
}