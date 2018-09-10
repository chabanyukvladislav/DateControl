using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Amporis.Xamarin.Forms.ColorPicker;
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
        public ObservableCollection<Event> NextDays { get; private set; }
        public ObservableCollection<Event> PreviosDays { get; private set; }
        public Event SelectedEvent { get; private set; }
        private GridView CurrentGridView { get; set; }
        private GridView BackGridView { get; set; }

        static Calendar()
        {
            MounthProperty = BindableProperty.Create(nameof(Mounth), typeof(Mounths), typeof(Calendar), (Mounths)(DateTime.Now.Month - 1));
            YearProperty = BindableProperty.Create(nameof(Year), typeof(int), typeof(Calendar), DateTime.Now.Year);
        }

        public Calendar()
        {
            _eventsCollection = EventsCollection.GetEventsCollection;
            _eventsCollection.CollectionChanged += CollectionChanged;
            Days = new ObservableCollection<Event>(GetDays(Mounth, Year));
            NextDays = new ObservableCollection<Event>(GetDays(Mounth == Mounths.December ? Mounths.January : Mounth + 1, Mounth == Mounths.December ? Year + 1 : Year));
            PreviosDays = new ObservableCollection<Event>(GetDays(Mounth == Mounths.January ? Mounths.December : Mounth - 1, Mounth == Mounths.January ? Year - 1 : Year));
            Up = new Command(ExecuteUp);
            Down = new Command(ExecuteDown);
            InitializeComponent();
            CurrentGridView = myGridView;
            BackGridView = myBackGridView;
            CurrentGridView.SelectedChanged += OnSelectedChanged;
            BackGridView.SelectedChanged += OnSelectedChanged;
            This.LowerChild(CurrentGridView);
            This.LowerChild(BackGridView);
        }

        private void CollectionChanged()
        {
            OnPropertyChanged("Collection");
        }

        private void ExecuteDown(object obj)
        {
            if (Mounth == Mounths.December)
            {
                Year += 1;
                Mounth = Mounths.January;
            }
            else
                Mounth += 1;
        }

        private void ExecuteUp(object obj)
        {
            if (Mounth == Mounths.January)
            {
                Year -= 1;
                Mounth = Mounths.December;
            }
            else
                Mounth -= 1;
        }

        private List<Event> GetDays(Mounths mounth, int year)
        {
            int date = GetStartDay(mounth, year);
            List<Event> list = new List<Event>();
            int[] arr = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int k = (int)mounth % 11;
            int count = arr[k];
            if (count == 28 && year % 4 == 0 &&
                (year % 100 != 0 || year % 400 == 0))
                count = 29;
            int last = date - 2;
            if (last < 0)
                last += 7;
            int m;
            if (mounth == Mounths.January)
                m = 11;
            else
                m = (int)mounth - 1;
            int lastCount = arr[m % 11];
            for (int i = lastCount - last + 1; i <= lastCount; i++)
            {
                Event element = _eventsCollection.GetEvent(i, (Mounths)m, m == 11 ? year - 1 : year);
                if (element == null)
                    element = new Event() { Day = i, Heh = Color.LightGray.ToHex(), Mounth = (Mounths)m, Year = m == 11 ? year - 1 : year, Description = "" };
                list.Add(element);
            }

            for (int i = 1; i <= count; ++i)
            {
                Event element = _eventsCollection.GetEvent(i, mounth, year);
                if (element == null)
                    element = new Event() { Day = i, Heh = Color.White.ToHex(), Mounth = mounth, Year = year, Description = "" };
                list.Add(element);
            }

            for (int i = 1; list.Count != 42; i++)
            {
                Mounths nextMounth = ((int)mounth) + 1 == 12 ? Mounths.January : mounth + 1;
                Event element = _eventsCollection.GetEvent(i, nextMounth, nextMounth == Mounths.January ? year + 1 : year);
                if (element == null)
                    element = new Event() { Day = i, Heh = Color.LightGray.ToHex(), Mounth = nextMounth, Year = nextMounth == Mounths.January ? year + 1 : year, Description = "" };
                list.Add(element);
            }
            return list;
        }

        private int GetStartDay(Mounths mounth, int year)
        {
            int mounthCode = GetMounthCode(mounth);
            int centuryCode = GetCenturyCode(year);
            int yearCode = (centuryCode + Convert.ToInt32(year.ToString().Remove(0, year.ToString().Length - 2)) +
                            (Convert.ToInt32(year.ToString().Remove(0, year.ToString().Length - 2)) / 4)) % 7;
            int date = (yearCode + mounthCode + 1) % 7;
            if (year % 4 == 0 &&
                (year % 100 != 0 || year % 400 == 0))
            {
                date -= 1;
                if (date == -1)
                    date = 6;
            }

            return date;
        }

        private int GetCenturyCode(int year)
        {
            int[] k = new[] { 6, 4, 2, 0 };
            int y = Convert.ToInt32(year.ToString().Remove(year.ToString().Length - 2));
            return k[(y - 16) % 4];
        }

        private int GetMounthCode(Mounths mounth)
        {
            int code = -1;
            switch (mounth)
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

        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                if (e.TotalY > 0)
                {
                    BackGridView.ItemsSource = PreviosDays;
                    BackGridView.TranslateTo(0, e.TotalY - BackGridView.Height);
                }
                else
                {
                    BackGridView.ItemsSource = NextDays;
                    BackGridView.TranslateTo(0, e.TotalY + BackGridView.Height);
                }
                CurrentGridView.TranslateTo(0, e.TotalY);
            }
            else
            {
                if (CurrentGridView.TranslationY < 200 && CurrentGridView.TranslationY > -200)
                {
                    CurrentGridView.TranslateTo(0, 0);
                    BackGridView.TranslateTo(0, 0);
                }
                else
                {
                    Animation animationBack = new Animation(el => BackGridView.TranslationY = el, BackGridView.TranslationY, 0);
                    animationBack.Commit(this, "AnimationBack", 16U, 2500U, Easing.CubicOut);

                    Animation animation = new Animation(el => CurrentGridView.TranslationY = el, CurrentGridView.TranslationY, CurrentGridView.TranslationY < 0 ? -CurrentGridView.Height : CurrentGridView.Height);
                    animation.Commit(this, "Animation", 16U, 2500U, Easing.CubicOut, Finished);
                }
            }
        }

        private void Finished(double arg1, bool arg2)
        {
            if (BackGridView.ItemsSource == NextDays)
            {
                PreviosDays = Days;
                Days = NextDays;
                Year = Mounth == Mounths.December ? Year + 1 : Year;
                Mounth = Mounth == Mounths.December ? Mounths.January : Mounth + 1;
                BackGridView.GestureRecognizers.Add(CurrentGridView.GestureRecognizers[0]);
                NextDays = new ObservableCollection<Event>(GetDays(Mounth == Mounths.December ? Mounths.January : Mounth + 1, Mounth == Mounths.December ? Year + 1 : Year));
                This.LowerChild(CurrentGridView);
                CurrentGridView.TranslateTo(0, 0);
                CurrentGridView.GestureRecognizers.Clear();
                GridView temp = CurrentGridView;
                CurrentGridView = BackGridView;
                BackGridView = temp;
            }
            else
            {
                NextDays = Days;
                Days = PreviosDays;
                Year = Mounth == Mounths.January ? Year - 1 : Year;
                Mounth = Mounth == Mounths.January ? Mounths.December : Mounth - 1;
                BackGridView.GestureRecognizers.Add(CurrentGridView.GestureRecognizers[0]);
                PreviosDays = new ObservableCollection<Event>(GetDays(Mounth == Mounths.January ? Mounths.December : Mounth - 1, Mounth == Mounths.January ? Year - 1 : Year));
                This.LowerChild(CurrentGridView);
                CurrentGridView.TranslateTo(0, 0);
                CurrentGridView.GestureRecognizers.Clear();
                GridView temp = CurrentGridView;
                CurrentGridView = BackGridView;
                BackGridView = temp;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == MounthProperty.PropertyName || propertyName == "Collection")
            {
                header.Text = Mounth + " " + Year;
                Days = new ObservableCollection<Event>(GetDays(Mounth, Year));
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