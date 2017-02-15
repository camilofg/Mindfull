using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MIndFull.Views
{
    public class Detail : ContentPage
    {
        public Detail()
        {
            StackLayout sl = new StackLayout();

            SearchBar sb = new SearchBar();

            sl.Children.Add(sb);

            Content = sl;
        }
    }
}
