using MIndFull.Models;
using MIndFull.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MIndFull
{
    public class App
    {
        public static INavigation Navigation { get; set; }
        public static MasterDetailPage MD { get; set; }
        public static NavigationPage NV { get; set; }
        public static User usuarioSesion { get; set; }

        public static Page GetMainPage()
        {
            var rootPage = new Login();
            Navigation = rootPage.Navigation;
            return rootPage;
        }
    }
}
