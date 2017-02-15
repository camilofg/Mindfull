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
    public partial class CreateActivity : ContentPage
    {
        //DatePicker datePicker;
        //StackLayout stack;
        public CreateActivity()
        {

            InitializeComponent();
            //datePicker = new DatePicker
            //{
            //    Format = "D",
            //    VerticalOptions = LayoutOptions.CenterAndExpand
            //};
            //stack = new StackLayout();
            //stack.Children.Add(datePicker);

            //Content = stack;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            var test = new Activity();
            test.ActDate = SelectedDate.Date.ToString();
            test.ActDesc = TxtActivityName.Text;
            test.ActState = 0;
            test.ActUserId = App.usuarioSesion.Id;
            
            //us.SessionUser = usuario;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://toolsoft.co/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync("Users/CreateActivity", new StringContent(JsonConvert.SerializeObject(test), Encoding.UTF8, "application/json")).Result;
            string content = await response.Content.ReadAsStringAsync();

            App.MD = new MasterDetailPage();
            App.MD.Master = new Master();
            App.NV = new NavigationPage(new GenericActivityView(0));
            App.MD.Detail = App.NV;
            App.MD.MasterBehavior = MasterBehavior.SplitOnLandscape;
            App.MD.IsPresented = false;

            Application.Current.MainPage = App.MD;
            //var data = JsonConvert.DeserializeObject<TokenUser>(content);

            //UserToken token = new UserToken();
            //token.UsID = data.SessionUser.Id;
            //token.UsToken = data.ValidationToken;

            //database.SaveToken(token);

            //return data;

            //NavigationPage detalle = null;
            //detalle = new NavigationPage(new CreateActivity());
            //if (detalle == null) return;
            //detalle.BarTextColor = Color.White;
            ////detalle.BarBackgroundColor = Color.Accent;
            //App.MD.IsPresented = false;
            //App.MD.Detail = detalle;

        }
    }
}
