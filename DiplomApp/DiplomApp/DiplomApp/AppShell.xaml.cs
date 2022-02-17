using System;
using System.Collections.Generic;

using DiplomApp.ViewModels;
using DiplomApp.Views;

using Xamarin.Forms;

namespace DiplomApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
