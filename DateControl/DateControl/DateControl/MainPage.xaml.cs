namespace DateControl
{
    public partial class MainPage
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
