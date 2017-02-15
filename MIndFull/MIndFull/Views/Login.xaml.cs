using MIndFull.Data;
using MIndFull.Models;
using MIndFull.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MIndFull.Views
{
    public partial class Login : ContentPage
    {
        User user;
        TokenDataBase database = new TokenDataBase();
        FacebookServices FBService = new FacebookServices();
        GoogleServices GoogleService = new GoogleServices();

        public Login()
        {
            InitializeComponent();
            if (App.usuarioSesion != null)
            {
                //waitActivityIndicator.IsRunning = false;
                App.MD = new MasterDetailPage();
                App.MD.Master = new Master();
                App.NV = new NavigationPage(new GenericActivityView(0));
                App.MD.Detail = App.NV;
                App.MD.MasterBehavior = MasterBehavior.SplitOnLandscape;
                App.MD.IsPresented = false;

                Application.Current.MainPage = App.MD;
            }

            NavigationPage.SetHasBackButton(this, false);
            this.user = new User();// usuario;

            BindingContext = this.user;
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if (s.GetType().Name == "Image")
                {
                    if (((Xamarin.Forms.FileImageSource)((Xamarin.Forms.Image)s).Source).File.Contains("facebook_button"))
                    {
                        string apiRequest = FBService.FBStringRequest();

                        var webView = new WebView
                        {
                            Source = apiRequest,
                            HeightRequest = 1
                        };

                        webView.Navigated += WebViewOnNavigated;

                        Content = webView;
                    }
                    else
                    {
                        string requestUrl = GoogleServices.GoogleStringRequest();

                        var webView = new WebView
                        {
                            Source = requestUrl,
                            HeightRequest = 1
                        };

                        webView.Navigated += WebViewGoogleOnNavigated;

                        Content = webView;
                    }
                }
                else
                {
                    if (((Xamarin.Forms.Label)s).Text == "New User?")
                        Navigation.PushModalAsync(new CreateUser());

                    else
                        Navigation.PushModalAsync(new ForgotPass());
                }
            };

            BtnLogin.Clicked += BtnLogin_Clicked;
            ImgBtnFB.GestureRecognizers.Add(tapGestureRecognizer);
            ImgBtnGoogle.GestureRecognizers.Add(tapGestureRecognizer);
            LblForgot.GestureRecognizers.Add(tapGestureRecognizer);
            LblNewUser.GestureRecognizers.Add(tapGestureRecognizer);

        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            var us = new User { Email = TxtUser.Text, UserName = TxtUser.Text, PasswordHash = TxtPass.Text };
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://toolsoft.co/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync("Users/ValidateUser/", new StringContent(JsonConvert.SerializeObject(us), Encoding.UTF8, "application/json")).Result;
            string content = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<User>(content);
            App.usuarioSesion = data;
            //var Usuario = user;

            //HttpClient client = new HttpClient();
            //Usuario.UserName = user.Email;
            //Usuario.PhoneNumber = user.PhoneNumber;
            //Usuario.UserName = user.UserName;

            //var tk = await CreateUserAsync(Usuario);

            //if (tk != null) {
            //    var content = new ContentPage
            //    {
            //        Title = "AuditOnSite",
            //        Content = new StackLayout
            //        {
            //            VerticalOptions = LayoutOptions.Center,
            //            Children = {
            //            new Label {
            //                HorizontalTextAlignment = TextAlignment.Center,
            //                Text = "Please check your email and confirm your account"
            //            }
            //        }
            //        }
            //    };
            //    await Navigation.PushAsync(content);
            //}
            App.MD = new MasterDetailPage();
            App.MD.Master = new Master();
            App.NV = new NavigationPage(new GenericActivityView(0));
            App.MD.Detail = App.NV;
            App.MD.MasterBehavior = MasterBehavior.SplitOnLandscape;
            App.MD.IsPresented = false;

            Application.Current.MainPage = App.MD;
        }

        

        #region Own
        //private async void BtnOwnLogin_OnClicked(object sender, EventArgs e)
        //{
        //    User usuario = new User();
        //    await Navigation.PushAsync(new MindFulnessApp.View.Login(usuario), true);//.Login(usuario));
        //}


        #endregion

        #region Google
        private async void WebViewGoogleOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var code = GoogleService.ExtractCodeFromUrl(e.Url);

            if (code != "")
            {
                var accessToken = await GoogleService.GetAccessTokenAsync(code);
                var googleUser = await GoogleService.GetGoogleUserProfileAsync(accessToken);

                if (googleUser != null)
                {
                    var us = new User();
                    us.Email = googleUser.Emails[0].Value;
                    us.UserName = googleUser.DisplayName;
                    us.Id = googleUser.Id;
                    us.EmailConfirmed = true;
                    var tk = DataHelper.CreateUserAsync(us);

                    App.MD = new MasterDetailPage();
                    App.MD.Master = new Master();
                    App.NV = new NavigationPage(new Detail());
                    App.MD.Detail = App.NV;
                    App.MD.MasterBehavior = MasterBehavior.SplitOnLandscape;
                    App.MD.IsPresented = true;

                    Application.Current.MainPage = App.MD;
                }
            }
        }
        #endregion

        #region Facebook
        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var accessToken = FBService.ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                var FBUser = await FBService.GetFacebookProfileAsync(accessToken);

                if (FBUser != null)
                {
                    var us = new User();
                    us.Email = FBUser.email;
                    us.Id = FBUser.id;
                    us.UserName = FBUser.name;
                    us.EmailConfirmed = true;
                    var tk = DataHelper.CreateUserAsync(us);

                    App.MD = new MasterDetailPage();
                    App.MD.Master = new Master();
                    App.NV = new NavigationPage(new Detail());
                    App.MD.Detail = App.NV;
                    App.MD.MasterBehavior = MasterBehavior.SplitOnLandscape;
                    App.MD.IsPresented = true;

                    Application.Current.MainPage = App.MD;
                }
            }
        }
        #endregion

    }
}
