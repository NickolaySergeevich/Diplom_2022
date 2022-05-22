using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MobileApp
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_login_Clicked(object sender, EventArgs e)
        {
            entry_login.Text = "test";
            entry_password.Text = "test";

            var username = entry_login.Text;
            var password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(entry_password.Text))).Replace("-", string.Empty);

            var request = WebRequest.Create("http://diplom.std-918.ist.mospolytech.ru/api/");
            var response = request.GetResponse();

            Console.WriteLine(response.GetResponseStream());
        }

        private void Button_registration_Clicked(object sender, EventArgs e)
        {

        }
    }
}
