using Amporis.Xamarin.Forms.ColorPicker;
using Xamarin.Forms;

namespace DateControl
{
    class MainPageViewModel
    {
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
                    item.Description = "";
                    item.Heh = Color.White.ToHex();
                    return;
                }
                await Application.Current.MainPage.Navigation.PushAsync(new AddEventPage(item));
            }
        }
    }
}
