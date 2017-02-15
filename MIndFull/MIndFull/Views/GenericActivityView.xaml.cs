using Acr.UserDialogs;
using MIndFull.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MIndFull.Views
{
    public partial class GenericActivityView : ContentPage
    {
        public ObservableCollection<Activity> ActivitiesList { get; set; }
        public GenericActivityView(int state)
        {
            InitializeComponent();
            ActivitiesList = new ObservableCollection<Activity>();
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("http://toolsoft.co/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var salida = cliente.GetStringAsync(String.Format("Users/GetUserActivities?userId={0}&state={1}", App.usuarioSesion.Id, state)).Result;
            List<Activity> ListaMenus = JsonConvert.DeserializeObject<List<Activity>>(salida);
            Title = state == 0 ? "Monkey's Mind Cage" : (state == 2 ? "Actions" : "Gotta do Today");
            var ListView0 = new ListView();
            var Stack = new StackLayout();
            ListView0.ItemTemplate = new DataTemplate(typeof(CustomActivityCell));
            ListView0.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {

                var ret = await DisplayActionSheet("Move To...", "Cancel", null, "Got to do today", "Program", "Delete");
                NavigationPage detalle = null;
                
                if (ret == "Got to do today")
                {

                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var actVal = (Activity)e.SelectedItem;
                    UpdateActivity updAct = new UpdateActivity();
                    updAct.ActId = actVal.ActId;
                    updAct.UnixActDate = unixTimestamp;

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://toolsoft.co/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.PostAsync("Users/SetDateToActivity", new StringContent(JsonConvert.SerializeObject(updAct), Encoding.UTF8, "application/json")).Result;
                    string content = await response.Content.ReadAsStringAsync();

                    detalle = new NavigationPage(new GenericActivityView(2));
                    //await Navigation.PushAsync(new GenericActivityView(2));
                }
                if (ret == "Program")
                {
                    detalle = new NavigationPage(new GenericActivityView(2));
                }
                else if (ret == "Delete")
                {
                    //var ActivitySelected = new Activity();
                    var ActivitySelected = ((Activity)e.SelectedItem);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://toolsoft.co/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.PostAsync("Users/DeleteActivity", new StringContent(JsonConvert.SerializeObject(ActivitySelected), Encoding.UTF8, "application/json")).Result;
                    string content = await response.Content.ReadAsStringAsync();

                    ActivitiesList.Remove((MIndFull.Activity)ListView0.SelectedItem);
                }
                //else {
                    // this.Navigation.PushAsync(new CreateActivity());
                    if (detalle == null) return;
                    detalle.BarTextColor = Color.White;
                    //detalle.BarBackgroundColor = Color.Accent;
                    App.MD.IsPresented = false;
                    App.MD.Detail = detalle;
                //}
            };


            foreach (var i in ListaMenus)
            {
                ActivitiesList.Add(i);
            }
            var btnAdd = new Button();
            btnAdd.Text = "Add Activity";
            btnAdd.Clicked += BtnAdd_Clicked;
            ListView0.ItemsSource = ActivitiesList;
            Stack.Children.Add(btnAdd);
            Stack.Children.Add(ListView0);

            Content = Stack;
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            NavigationPage detalle = null;
            detalle = new NavigationPage(new CreateActivity()); 
            if (detalle == null) return;
            detalle.BarTextColor = Color.White;
            //detalle.BarBackgroundColor = Color.Accent;
            App.MD.IsPresented = false;
            App.MD.Detail = detalle;

        }


    }

    public class CustomActivityCell : ViewCell
    {
        public CustomActivityCell()
        {
            var nameLabel = new Label();
            var horizontalLayout = new StackLayout();

            //set bindings
            nameLabel.SetBinding(Label.TextProperty, new Binding("ActDesc"));

            //Set properties for desired design
            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.HorizontalOptions = LayoutOptions.Fill;
            nameLabel.FontSize = 24;

            horizontalLayout.Children.Add(nameLabel);


            View = horizontalLayout;
        }
    }
}
