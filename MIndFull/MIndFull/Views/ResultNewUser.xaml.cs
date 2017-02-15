using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MIndFull.Views
{
    public partial class ResultNewUser : ContentPage
    {
        public ResultNewUser(string message)
        {
            InitializeComponent();
            LblResult.Text = message;
        }
    }
}
