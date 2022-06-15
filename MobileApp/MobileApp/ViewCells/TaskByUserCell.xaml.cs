
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskByUserCell
    {
        public TaskByUserCell()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            ((Button)sender).Text = "В разработке";
        }
    }
}