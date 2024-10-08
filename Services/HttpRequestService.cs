﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Kvizazov
{
    public class HttpRequestService
    {
        public async Task<string> HttpGetRequest(string route)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://kvizazov-app-default-rtdb.firebaseio.com/{route}.json");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }

        public async Task<string> HttpPatchRequest(string route, string json)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://kvizazov-app-default-rtdb.firebaseio.com/{route}.json")
                {
                    Content = content
                };
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }

        public async Task<string> HttpGetRequestWithFilter(string route, string filterKey, string filterValue)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://kvizazov-app-default-rtdb.firebaseio.com/{route}.json?orderBy={filterKey}&equalTo={filterValue}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }

        public async Task<string> HttpGetRequestOnlyLastElement(string route, string filterKey)
        {
           using (HttpClient client = new HttpClient()){
                HttpResponseMessage response = await client.GetAsync($"https://kvizazov-app-default-rtdb.firebaseio.com/{route}.json?orderBy=\"{filterKey}\"&limitToLast=1");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }

        public async Task<string> HttpDeleteRequest(string route)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"https://kvizazov-app-default-rtdb.firebaseio.com/{route}.json");
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
    }
}
