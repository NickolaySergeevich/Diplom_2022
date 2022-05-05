using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_login_Clicked(object sender, EventArgs e)
        {
            entry_login.Text = "test";
            entry_password.Text = "test";

            string username = entry_login.Text;
            string password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(entry_password.Text))).Replace("-", string.Empty);

            WebRequest request = WebRequest.Create("http://diplom.std-918.ist.mospolytech.ru/api/login/");
            WebResponse response = request.GetResponse();

            Console.WriteLine(password);
            Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }

        private void Button_registration_Clicked(object sender, EventArgs e)
        {

        }
    }
}
