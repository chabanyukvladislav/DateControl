using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Amporis.Xamarin.Forms.ColorPicker;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xamarin.Forms;

namespace DateControl.Calendar
{
    public partial class Calendar
    {
        private readonly IEventsCollection _eventsCollection;

        public static readonly BindableProperty MounthProperty;
        public static readonly BindableProperty YearProperty;

        public ICommand Up { get; set; }
        public ICommand Down { get; set; }

        public Mounths Mounth
        {
            get => (Mounths)GetValue(MounthProperty);
            set => SetValue(MounthProperty, value);
        }
        public int Year
        {
            get => (int)GetValue(YearProperty);
            set => SetValue(YearProperty, value);
        }

        public ObservableCollection<Event> Days { get; private set; }
        public Event SelectedEvent { get; private set; }

        static Calendar()
        {
            MounthProperty = BindableProperty.Create(nameof(Mounth), typeof(Mounths), typeof(Calendar), (Mounths)(DateTime.Now.Month - 1));
            YearProperty = BindableProperty.Create(nameof(Year), typeof(int), typeof(Calendar), DateTime.Now.Year);
        }

        public Calendar()
        {
            _eventsCollection = EventsCollection.GetEventsCollection;
            _eventsCollection.CollectionChanged += CollectionChanged;
            Days = new ObservableCollection<Event>(GetDays());
            Up = new Command(ExecuteUp);
            Down = new Command(ExecuteDown);
            InitializeComponent();
            myGridView.SelectedChanged += OnSelectedChanged;
        }

        private void CollectionChanged()
        {
            OnPropertyChanged("Collection");
        }

        private void ExecuteDown(object obj)
        {
            if (Mounth == Mounths.December)
            {
                Mounth = Mounths.January;
                Year += 1;
            }
            else
                Mounth += 1;
        }

        private void ExecuteUp(object obj)
        {
            if (Mounth == Mounths.January)
            {
                Mounth = Mounths.December;
                Year -= 1;
            }
            else
                Mounth -= 1;
        }

        private List<Event> GetDays()
        {
            int date = GetStartDay();
            List<Event> list = new List<Event>();
            int[] arr = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int k = (int)Mounth % 11;
            int count = arr[k];
            if (count == 28 && Year % 4 == 0 &&
                (Year % 100 != 0 || Year % 400 == 0))
                count = 29;
            int last = date - 2;
            if (last < 0)
                last += 7;
            int m;
            if (Mounth == Mounths.January)
                m = 11;
            else
                m = (int)Mounth - 1;
            int lastCount = arr[m % 11];
            for (int i = lastCount - last + 1; i <= lastCount; i++)
            {
                Event element = _eventsCollection.GetEvent(i, (Mounths)m, m == 11 ? Year - 1 : Year);
                if (element == null)
                    element = new Event() { Day = i, Heh = Color.White.ToHex(), Mounth = (Mounths)m, Year = m == 11 ? Year - 1 : Year, Description = ""};
                list.Add(element);
            }

            for (int i = 1; i <= count; ++i)
            {
                Event element = _eventsCollection.GetEvent(i, Mounth, Year);
                if (element == null)
                    element = new Event() { Day = i, Heh = Color.White.ToHex(), Mounth = Mounth, Year = Year, Description = "" };
                list.Add(element);
            }

            for (int i = 1; list.Count != 42; i++)
            {
                Mounths mounth = ((int) Mounth) + 1 == 12 ? Mounths.January : Mounth + 1;
                Event element = _eventsCollection.GetEvent(i, mounth, mounth == Mounths.January ? Year + 1 : Year);
                if (element == null)
                    element = new Event() { Day = i, Heh = Color.White.ToHex(), Mounth = mounth, Year = mounth == Mounths.January ? Year + 1 : Year, Description = "" };
                list.Add(element);
            }
            return list;
        }

        private int GetStartDay()
        {
            int mounthCode = GetMounthCode();
            int centuryCode = GetCenturyCode();
            int yearCode = (centuryCode + Convert.ToInt32(Year.ToString().Remove(0, Year.ToString().Length - 2)) +
                            (Convert.ToInt32(Year.ToString().Remove(0, Year.ToString().Length - 2)) / 4)) % 7;
            int date = (yearCode + mounthCode + 1) % 7;
            if (Year % 4 == 0 &&
                (Year % 100 != 0 || Year % 400 == 0))
            {
                date -= 1;
                if (date == -1)
                    date = 6;
            }

            return date;
        }

        private int GetCenturyCode()
        {
            int[] k = new[] { 6, 4, 2, 0 };
            int year = Convert.ToInt32(Year.ToString().Remove(Year.ToString().Length - 2));
            return k[(year - 16) % 4];
        }

        private int GetMounthCode()
        {
            int code = -1;
            switch (Mounth)
            {
                case Mounths.January:
                    code = 1;
                    break;
                case Mounths.February:
                    code = 4;
                    break;
                case Mounths.March:
                    code = 4;
                    break;
                case Mounths.April:
                    code = 0;
                    break;
                case Mounths.May:
                    code = 2;
                    break;
                case Mounths.June:
                    code = 5;
                    break;
                case Mounths.July:
                    code = 0;
                    break;
                case Mounths.August:
                    code = 3;
                    break;
                case Mounths.September:
                    code = 6;
                    break;
                case Mounths.October:
                    code = 1;
                    break;
                case Mounths.November:
                    code = 4;
                    break;
                case Mounths.December:
                    code = 6;
                    break;
            }

            return code;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == MounthProperty.PropertyName || propertyName == YearProperty.PropertyName || propertyName == "Collection")
            {
                header.Text = Mounth + " " + Year;
                Days = new ObservableCollection<Event>(GetDays());
                myGridView.ItemsSource = Days;
            }
        }

        private void OnSelectedChanged(Event item)
        {
            SelectedEvent = item;
            SelectedChanged?.Invoke(item);
        }

        public event Action<Event> SelectedChanged;
    }
}