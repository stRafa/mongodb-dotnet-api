using MongoDB.Bson.IO;
using MongoDBAPI.Models;
using System.Text.Json;

namespace MongoDBAPI.Services
{
    public class UsersService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public UsersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _url = "https://reqres.in/api/users";
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var uri = await _httpClient.GetAsync(_url);
            var json = await uri.Content.ReadAsStringAsync();
            var root = JsonSerializer.Deserialize<RootObject>(json);
            return root.data;
        }
        
    }
    public class RootObject
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<User> data { get; set; }
        public dynamic support { get; set; }
    }

}
