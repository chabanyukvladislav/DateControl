using Xamarin.Forms;
using DateControl.Calendar;

namespace DateControl
{
    class MainPageViewModel
    {
        private readonly IEventsCollection _eventsCollection;

        public MainPageViewModel()
        {
            _eventsCollection = EventsCollection.GetEventsCollection;
        }

        public async void SelectedChanged(Event item)
        {
            if (item.Description == "")
                await Application.Current.MainPage.Navigation.PushAsync(new AddEventPage(item));
            else
            {
                if (await Application.Current.MainPage.DisplayAlert("Message", "What would you want to do?", "Edit/Delete",
                    "Cancel") == false)
                    return;
                if (await Application.Current.MainPage.DisplayAlert("Message", "What would you want to do?", "Edit",
                        "Delete") == false)
                {
                    _eventsCollection.DeleteEvent(item);
                    return;
                }
                await Application.Current.MainPage.Navigation.PushAsync(new AddEventPage(item));
            }
        }
    }
}
