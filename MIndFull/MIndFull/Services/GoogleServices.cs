using MIndFull.Data;
using MIndFull.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MIndFull.Services
{
    public class GoogleServices
    {
        public static readonly string ClientId = "648876270296-aonh1it6g8cen4qbf08g571ftkm3367q.apps.googleusercontent.com";
        public static readonly string ClientSecret = "QlLkDF_chUSaft6UnhVdtKEU";
        public static readonly string RedirectUri = "http://toolsoft.co/";

        TokenDataBase database = new TokenDataBase();

        public static string GoogleStringRequest()
        {
            return "https://accounts.google.com/o/oauth2/v2/auth?"
                                            + "scope=email%20profile"
                                            + "&redirect_uri=" + RedirectUri
                                            + "&response_type=code "
                                            + "&client_id=" + ClientId;
        }

        public async Task<string> GetAccessTokenAsync(string codigo)
        {
            var requestUrl =
            "https://www.googleapis.com/oauth2/v4/token/"
            + "?code=" + codigo.Substring(0, codigo.Length - 1)
            + "&client_id=" + ClientId
            + "&client_secret=" + ClientSecret
            + "&redirect_uri=" + RedirectUri
            + "&grant_type=authorization_code";

            var httpClient = new HttpClient();

            var response = await httpClient.PostAsync(requestUrl, null);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();

                var accessToken = JsonConvert.DeserializeObject<JObject>(json).Value<string>("access_token");

                UserToken tk = new UserToken();
                tk.UsID = "cualquier cosa2";
                tk.UsToken = accessToken;

                database.SaveToken(tk);
                //SaveToken(tk);

                return accessToken;
            }

            return null;
        }

        public async Task<GoogleProfile> GetGoogleUserProfileAsync(string accessToken)
        {
            var tk = database.GetToken(1);
            var requestUrl = "https://www.googleapis.com/plus/v1/people/me"
                             + "?access_token=" + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var googleProfile = JsonConvert.DeserializeObject<GoogleProfile>(userJson);

            return googleProfile;
        }


        public string ExtractCodeFromUrl(string url)
        {
            if (url.Contains("code="))
            {
                var attributes = url.Split('&');
                var code = attributes.FirstOrDefault(s => s.Contains("code=")).Split('=')[1];
                return code;
            }

            return string.Empty;
        }
    }
}
