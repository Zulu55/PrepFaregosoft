using Newtonsoft.Json;
using PrepFaregosoft.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrepFaregosoft.Helpers
{
    public class ApiService
    {
        public static async Task<Response> LoginAsync(string urlBase, string servicePrefix, string controller, string email, string password)
        {
            try
            {
                string request = JsonConvert.SerializeObject(new LoginRequestModel
                {
                    Email = email,
                    Password = password
                });

                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                string url = $"{servicePrefix}/{controller}/Login";

                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(urlBase)
                };

                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                User user = JsonConvert.DeserializeObject<User>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = user
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
