using MIndFull.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MIndFull.Data
{
    public class DataHelper
    {
        static TokenDataBase database = new TokenDataBase();
        public static async Task<TokenUser> CreateUserAsync(User usuario)
        {
            TokenUser us = new TokenUser();
            us.SessionUser = usuario;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://toolsoft.co/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync("Users/PostUser", new StringContent(JsonConvert.SerializeObject(us), Encoding.UTF8, "application/json")).Result;
            string content = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<TokenUser>(content);

            UserToken token = new UserToken();
            token.UsID = data.SessionUser.Id;
            token.UsToken = data.ValidationToken;

            database.SaveToken(token);

            return data;

        }
    }
}
