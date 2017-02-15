using MIndFull.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MIndFull
{
    public class OpcionesMenuViewModel : BaseViewModel
    {
        public ObservableCollection<OpcionesMenu> ListaOpciones { get; set; }
        TokenDataBase database;
        public OpcionesMenuViewModel()
        {
            database = new TokenDataBase();
            var localMenu = database.GetUserMenu();
            if (localMenu == null)
            {
                foreach (var i in localMenu)
                {
                    ListaOpciones.Add(i);
                }
            }
            else
            {
                var ListaMenus = new List<OpcionesMenu>();
                ListaOpciones = new ObservableCollection<OpcionesMenu>();
                HttpClient cliente = new HttpClient();
                cliente.BaseAddress = new Uri("http://toolsoft.co/");
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var salida = cliente.GetStringAsync("Users/GetMenu").Result;
                ListaMenus = JsonConvert.DeserializeObject<List<OpcionesMenu>>(salida);
                //Icon = "slideout.png";
                database.SaveUserMenu(ListaMenus);
                foreach (var i in ListaMenus)
                {
                    ListaOpciones.Add(i);
                }
            }
        }
    }
}
