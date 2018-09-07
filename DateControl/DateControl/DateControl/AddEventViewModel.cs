using System;
using System.ComponentModel;
using System.Windows.Input;
using Amporis.Xamarin.Forms.ColorPicker;
using Xamarin.Forms;
using DateControl.Calendar;

namespace DateControl
{
    class AddEventViewModel: INotifyPropertyChanged
    {
        private readonly Event _item;
        private readonly IEventsCollection _eventsCollection;
        private Color _color;

        public ICommand Ok { get; }
        public string Description { get; set; }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
            }
        }

        public AddEventViewModel(Event item)
        {
            _eventsCollection = EventsCollection.GetEventsCollection;
            _item = item;
            Ok = new Command(ExecuteOk);
            Description = item.Description;
            Color = Color.FromHex(item.Heh);
        }

        private async void ExecuteOk()
        {
            _item.Description = Description;
            _item.Heh = Color.ToHex();
            if (_item.Id == Guid.Empty)
                _eventsCollection.AddEvent(_item);
            else
                _eventsCollection.EditEvent(_item);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
