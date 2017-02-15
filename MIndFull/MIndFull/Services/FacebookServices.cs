using MIndFull.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MIndFull.Services
{
    public class FacebookServices
    {
        private string Client_Id = "258674577836404";

        public string FBStringRequest()
        {
            return "https://www.facebook.com/v2.8/dialog/oauth?client_id="
                                    + Client_Id
                                    + "&display=popup&response_type=token&redirect_uri=https://www.facebook.com/connect/login_success.html"
                                    + "&scope=email,user_friends";
        }

        public async Task<FB_Profile> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.7/me/"
                            + "?fields=name,picture,cover,age_range,devices{os},email,gender,is_verified"
                            + "&access_token=" + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var us = JsonConvert.DeserializeObject<FB_Profile>(userJson);
            return us;
            /*
            HttpClient client = new HttpClient();

            User Usuario = new User();
            Usuario.Id = us.id;
            Usuario.EmailConfirmed = us.is_verified;
            Usuario.Email = us.email;
            Usuario.UserName = us.name;

            var json = JsonConvert.SerializeObject(Usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = "http://toolsoft.co/Users/PostUser";
            HttpResponseMessage response = null;
            try
            {
                response = await client.PostAsync(uri, content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return "Prueba de token";*/
        }

        public string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace("http://www.facebook.com/connect/login_success.html#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&expires_in="));
                return accessToken;
            }
            return string.Empty;
        }

    }
}
