using System;
using System.Collections.Generic;
using System.ComponentModel;

using DiplomApp.Models;
using DiplomApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiplomApp.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}