using MIndFull.Data;
using MIndFull.Models;
using MIndFull.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MIndFull
{
    public class MasterMainView : MasterDetailPage
    {
        TokenDataBase database;
        User usuario;
        private OpcionesMenuViewModel ViewModel
        {
            get { return BindingContext as OpcionesMenuViewModel; }
        }

        public MasterMainView()
        {
            Title = "Master";
            database = new TokenDataBase();
            var usToken = database.GetLastToken();
            BindingContext = new OpcionesMenuViewModel();

            Xamarin.Forms.Image logo = new Xamarin.Forms.Image
            {
                Source = "iconmeditation.png"
            };

            Label header = new Label
            {
                Text = "MindFulness Hub",
                TextColor = Color.Black,
                Font = Font.SystemFontOfSize(22),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var Cabecera = new StackLayout
            {
                Spacing = 4,
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Horizontal,
                Children = { logo, header }
            };

            ScrollView contenedor = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = ScrollOrientation.Vertical
            };

            ListView listaOpciones = new ListView();
            listaOpciones.ItemsSource = database.GetLastToken() != null ? ViewModel.ListaOpciones : null;
            listaOpciones.ItemTemplate = new DataTemplate(typeof(OpcionesCell));
            listaOpciones.RowHeight = Device.OnPlatform(28, 28, 36);

            listaOpciones.ItemSelected += ListaOpciones_ItemSelected;

            contenedor.Content = new StackLayout
            {
                BackgroundColor = Color.White,
                Children =
              {
                  Cabecera, listaOpciones
              }
            };

            this.Master = new ContentPage
            {
                BackgroundColor = Color.White,
                Title = "=",
                Padding = new Thickness(10, Device.OnPlatform(30, 15, 15), 10, 10),
                Content = contenedor
            };

            var usuario = new User();
            var detail = new NavigationPage(new Login());
            if (usToken != null)
            {
                if (validate(usToken) != null)
                    detail = new NavigationPage(new GenericActivityView(0));
            }
            this.Detail = detail;
            Master.Icon = "slideout.png";
        }

        private async Task<string> validate(UserToken usToken) {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://toolsoft.co/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync("Users/ValidateEmail", new StringContent(JsonConvert.SerializeObject(usToken), Encoding.UTF8, "application/json")).Result;
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
        private void ListaOpciones_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            NavigationPage detalle = null;

            var item = e.SelectedItem as OpcionesMenu;
            switch (item.Name)
            {
                case "Acerca de": detalle = new NavigationPage(new AboutPage()); break;
                case "Monkey's MindCage": detalle = new NavigationPage(new GenericActivityView(0)); break;
                case "Gotta do Today": detalle = new NavigationPage(new GenericActivityView(1)); break;
                case "Actions": detalle = new NavigationPage(new GenericActivityView(2)); break;
                case "Log Out": database.DeleteAll(); detalle = new NavigationPage(new AboutPage()); break;
            }
            if (detalle == null) return;
            detalle.BarTextColor = Color.White;
            //detalle.BarBackgroundColor = Color.Accent;
            this.IsPresented = false;
            //IsPresented = false;

            this.Detail = detalle;
        }

    }
}
