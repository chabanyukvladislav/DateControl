using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace DateControl.Calendar
{
    public partial class Calendar
    {
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

        public ObservableCollection<int> Days { get; private set; }

        static Calendar()
        {
            MounthProperty = BindableProperty.Create(nameof(Mounth), typeof(Mounths), typeof(Calendar), (Mounths)(DateTime.Now.Month - 1));
            YearProperty = BindableProperty.Create(nameof(Year), typeof(int), typeof(Calendar), DateTime.Now.Year);
        }

        public Calendar()
        {
            Days = new ObservableCollection<int>(GetDays());
            Up = new Command(ExecuteUp);
            Down = new Command(ExecuteDown);
            InitializeComponent();
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

        private List<int> GetDays()
        {
            int date = GetStartDay();
            List<int> list = new List<int>();
            int[] arr = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int k = (int)Mounth % 11;
            int count = arr[k];
            if (count == 28 && Year % 4 == 0 &&
                (Year % 100 != 0 || Year % 400 == 0))
                count = 29;
            int last = date - 2;
            if (last < 0)
                last += 7;
            int lastCount = arr[((int)Mounth - 1) % 11];
            for (int i = lastCount - last + 1; i <= lastCount; i++)
            {
                list.Add(i);
            }

            for (int i = 1; i <= count; ++i)
            {
                list.Add(i);
            }

            for (int i = 1; list.Count != 42; i++)
            {
                list.Add(i);
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

            if (propertyName == MounthProperty.PropertyName || propertyName == YearProperty.PropertyName)
            {
                Days = new ObservableCollection<int>(GetDays());
                
            }
        }
    }
}