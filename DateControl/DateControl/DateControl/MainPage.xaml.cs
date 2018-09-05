using Xamarin.Forms;

namespace DateControl
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            MainPageViewModel viewModel = new MainPageViewModel();
            calendar.SelectedChanged += viewModel.SelectedChanged;
            BindingContext = viewModel;
        }
    }
}
