using DateControl.Calendar;
using Xamarin.Forms;

namespace DateControl
{
    public partial class AddEventPage: ContentPage
    {
        public AddEventPage(Event item)
        {
            InitializeComponent();
            AddEventViewModel vm = new AddEventViewModel(item);
            BindingContext = vm;
        }
    }
}