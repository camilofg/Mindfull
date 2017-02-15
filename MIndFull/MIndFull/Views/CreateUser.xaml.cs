using MIndFull.Data;
using MIndFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MIndFull.Views
{
    public partial class CreateUser : ContentPage
    {
        public CreateUser()
        {
            InitializeComponent();
        }


        private async void BtnCreate_Clicked(object sender, EventArgs e)
        {
            waitActivityIndicator.IsRunning = true;
            var us = new User();
            us.Email = TxtEmail.Text;
            us.UserName = TxtEmail.Text;
            us.Id = Guid.NewGuid().ToString();
            us.PasswordHash = TxtPass.Text;
            us.PhoneNumber = TxtPhone.Text;
            var tk = await DataHelper.CreateUserAsync(us);

            if (tk != null)
            {
                waitActivityIndicator.IsRunning = false;
                await Navigation.PushModalAsync(new ResultNewUser(tk.Msg));
            }
        }
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
    }
}
