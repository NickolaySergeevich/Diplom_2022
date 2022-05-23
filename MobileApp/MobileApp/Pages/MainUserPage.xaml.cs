using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainUserPage
    {
        private readonly RestService _restService;

        public MainUserPage()
        {
            InitializeComponent();

            _restService = new RestService();
        }
    }
}