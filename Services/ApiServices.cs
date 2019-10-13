using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BloodTrace.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BloodTrace.Services
{
    class ApiServices
    {
        //Define all the rest api we will use in the project

        public async Task<bool> RegisterUser(string email, string password, string confirmPassword)
        {
            var registerModel = new RegisterModel()
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            //need HTTPClient to communicate with server.  Without it we cannot call this api in Xamarin forms.
            var httpClient = new HttpClient();
            //Serialize and Deserialize objects.  Meaning change from C# to JSON and vice versa.
            //Pass in your OBJECT that will need conversion. In this case you created an object and stored it in registerModel
            var json = JsonConvert.SerializeObject(registerModel);
            //creates formatted text for http server communication
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://jtbloodtrace.azurewebsites.net/api/Account/Register", content);
            return response.IsSuccessStatusCode;
        }

        //create login method
        public async Task<bool> LoginUser(string email, string password)
        {
            var keyvalues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username", email),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password"),
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://jtbloodtrace.azurewebsites.net/Token");
            request.Content = new FormUrlEncodedContent(keyvalues);
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(content);
            var accessToken = jObject.Value<string>("access_token");
            Settings.AccessToken = accessToken;
            Settings.UserName = email;
            Settings.Password = password;

            return response.IsSuccessStatusCode;
        }

        public async Task<List<BloodUser>> FindBlood(String country, string bloodType)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Settings.AccessToken);
            //First way to get access token using query string parameters
            //httpClient.GetStringAsync("jtbloodtrace.azurewebsites.net/api/BloodUsers?bloodGroup=A%2b&country=USA" + bloodType + "&country =" + country);
            var bloodApiUrl = "https://jtbloodtrace.azurewebsites.net/api/BloodUsers";
            //Second way
            var json = await httpClient.GetStringAsync($"{bloodApiUrl}?bloodGroup={bloodType}&country={country}");
            return JsonConvert.DeserializeObject<List<BloodUser>>(json);
        }

        public async Task<List<BloodUser>> LatestBloodUser()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Settings.AccessToken);
            //First way to get access token using query string parameters
            //httpClient.GetStringAsync("jtbloodtrace.azurewebsites.net/api/BloodUsers?bloodGroup=A%2b&country=USA" + bloodType + "&country =" + country);
            var bloodApiUrl = "https://jtbloodtrace.azurewebsites.net/api/BloodUsers";
            //Second way
            var json = await httpClient.GetStringAsync(bloodApiUrl);
            return JsonConvert.DeserializeObject<List<BloodUser>>(json);
        }

        public async Task<bool> RegisterDonor(BloodUser bloodUser)
        {
            var json = JsonConvert.SerializeObject(bloodUser);
            var httpClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Settings.AccessToken);
            //First way to get access token using query string parameters
            //httpClient.GetStringAsync("jtbloodtrace.azurewebsites.net/api/BloodUsers?bloodGroup=A%2b&country=USA" + bloodType + "&country =" + country);
            var bloodApiUrl = "https://jtbloodtrace.azurewebsites.net/api/BloodUsers";

            var response = await httpClient.PostAsync(bloodApiUrl, content);
            return response.IsSuccessStatusCode;
        }


    }
}
