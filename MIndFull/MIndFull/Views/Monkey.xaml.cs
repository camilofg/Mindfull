using Acr.UserDialogs;
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

namespace MIndFull
{
    public partial class Monkey : ContentPage
    {

        public ObservableCollection<Activity> ListaOpciones { get; set; }
        public Monkey(int state)
        {
            InitializeComponent();
            ListaOpciones = new ObservableCollection<Activity>();
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("http://toolsoft.co/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var salida = cliente.GetStringAsync("Users/GetUserActivities?userId=d0cce867-cdaa-40fd-ba37-a09a1e7b802c").Result;
            List<Activity> ListaMenus = JsonConvert.DeserializeObject<List<Activity>>(salida);
            this.Title = state == 0 ? "Monkey's Mind Cage" : (state == 1 ? "Actions" : "Gotta do Today");
            ListView0.ItemTemplate = new DataTemplate(typeof(CustomActivityCell));
            ListView0.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {

                var ret = await DisplayActionSheet("Move To...", "Cancel", null, new string[] { "Got to do today", "Program", "Delete" });

                if (ret == "Edit")
                {

                    PromptConfig promptConfig = new PromptConfig();
                    promptConfig.CancelText = "CANCEL";
                    promptConfig.InputType = InputType.Number;
                    promptConfig.Message = "Modify QTA";
                    promptConfig.OkText = "OK";
                    promptConfig.Title = "UPDATE";
                    PromptResult result = await UserDialogs.Instance.PromptAsync(promptConfig);
                    //if (result.Ok)
                    //    ((VeggieViewModel)lstView.SelectedItem) = int.Parse(result.Value);

                }
                else if (ret == "Delete")
                {

                    //veggies.Remove((VeggieViewModel)lstView.SelectedItem);
                    //App.List.Remove((Model)lv.SelectedItem);

                }
                else { }
            };


            foreach (var i in ListaMenus)
            {
                ListaOpciones.Add(i);
            }

            ListView0.ItemsSource = ListaOpciones;


        }

        //private void SelectItem(object sender, EventArgs e) {
        //    DisplayActionSheet("Move To:", "Cancel", null, "Actions", "Got To Do Today", "Program");
        //}
    }


    public class CustomActivityCell : ViewCell
    {
        public CustomActivityCell()
        {
            var nameLabel = new Label();
            //var dateLabel = new Label();
            var horizontalLayout = new StackLayout();

            //set bindings
            nameLabel.SetBinding(Label.TextProperty, new Binding("ActDesc"));
            //dateLabel.SetBinding(Label.TextProperty, new Binding("ActDate"));

            //Set properties for desired design
            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.HorizontalOptions = LayoutOptions.Fill;
            nameLabel.FontSize = 24;
            //dateLabel.HorizontalOptions = LayoutOptions.End;

            horizontalLayout.Children.Add(nameLabel);
            //horizontalLayout.Children.Add(dateLabel);
            // add to parent view
            View = horizontalLayout;

        }
    }

}
