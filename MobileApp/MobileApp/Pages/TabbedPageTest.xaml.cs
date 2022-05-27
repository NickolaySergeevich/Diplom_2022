using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageTest : TabbedPage
    {
        private ObservableCollection<ClassTest> _classTests;

        public ObservableCollection<ClassTest> ClassTests
        {
            get
            {
                return _classTests ?? (_classTests = new ObservableCollection<ClassTest>());
            }
        }

        public TabbedPageTest()
        {
            InitializeComponent();
            listView_main.ItemsSource = ClassTests;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var temp = new ClassTest()
            {
                Name = "test name",
                Description = "test desc"
            };

            ClassTests.Add(temp);
        }
    }
}