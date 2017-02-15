using MIndFull.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MIndFull.Views
{
    public class Master : ContentPage
    {
        TokenDataBase database;
        private OpcionesMenuViewModel ViewModel
        {
            get { return BindingContext as OpcionesMenuViewModel; }
        }
        public Master()
        {
            Title = "MASTER";
            Icon = "slideout.png";
            BackgroundColor = Color.White;
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

            StackLayout sl = new StackLayout();
            ListView listView = new ListView();
            listView.ItemSelected += ListView_ItemSelected;
            listView.ItemTemplate = new DataTemplate(typeof(OpcionesCell));
            listView.RowHeight = Device.OnPlatform(28, 28, 36);
            listView.ItemsSource = ViewModel.ListaOpciones;
            sl.Children.Add(Cabecera);
            sl.Children.Add(listView);
            Content = sl;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            NavigationPage detalle = null;

            var item = e.SelectedItem as OpcionesMenu;
            switch (item.Name)
            {
                case "About": detalle = new NavigationPage(new AboutPage()); break;
                case "Monkey's MindCage": detalle = new NavigationPage(new GenericActivityView(0)); break;
                case "Gotta do Today": detalle = new NavigationPage(new GenericActivityView(1)); break;
                case "Actions": detalle = new NavigationPage(new GenericActivityView(2)); break;
                case "Log Out": database.DeleteAll(); detalle = new NavigationPage(new AboutPage()); break;
            }
            if (detalle == null) return;
            detalle.BarTextColor = Color.White;
            App.MD.IsPresented = false;
            App.NV = detalle;
            App.MD.Detail = App.NV;
        }
    }
}
