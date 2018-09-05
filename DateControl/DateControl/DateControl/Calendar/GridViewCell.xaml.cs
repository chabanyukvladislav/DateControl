using System.Globalization;
using Xamarin.Forms;

namespace DateControl.Calendar
{
    public partial class GridViewCell
    {
        public static readonly BindableProperty EventProperty;
        public static readonly BindableProperty BorderBackgroundProperty;
        public static readonly BindableProperty LeftMarginProperty;
        public static readonly BindableProperty TopMarginProperty;
        public static readonly BindableProperty RightMarginProperty;
        public static readonly BindableProperty BottomMarginProperty;
        
        public Color BorderBackground
        {
            get => (Color)GetValue(BorderBackgroundProperty);
            set => SetValue(BorderBackgroundProperty, value);
        }
        public int LeftMargin
        {
            get => (int)GetValue(LeftMarginProperty);
            set => SetValue(LeftMarginProperty, value);
        }
        public int TopMargin
        {
            get => (int)GetValue(TopMarginProperty);
            set => SetValue(TopMarginProperty, value);
        }
        public int RightMargin
        {
            get => (int)GetValue(RightMarginProperty);
            set => SetValue(RightMarginProperty, value);
        }
        public int BottomMargin
        {
            get => (int)GetValue(BottomMarginProperty);
            set => SetValue(BottomMarginProperty, value);
        }
        public Event Event
        {
            get => (Event)GetValue(EventProperty);
            set => SetValue(EventProperty, value);
        }

        static GridViewCell()
        {
            EventProperty = BindableProperty.Create(nameof(Event), typeof(Event), typeof(GridViewCell), new Event());
            BorderBackgroundProperty = BindableProperty.Create(nameof(BorderBackground), typeof(Color), typeof(GridViewCell), Color.Black);
            LeftMarginProperty = BindableProperty.Create(nameof(LeftMargin), typeof(int), typeof(GridViewCell), 0);
            TopMarginProperty = BindableProperty.Create(nameof(TopMargin), typeof(int), typeof(GridViewCell), 1);
            RightMarginProperty = BindableProperty.Create(nameof(RightMargin), typeof(int), typeof(GridViewCell), 1);
            BottomMarginProperty = BindableProperty.Create(nameof(BottomMargin), typeof(int), typeof(GridViewCell), 0);
        }

        public GridViewCell(Event item, int leftMargin = 0, int topMargin = 1, int rightMargin = 1, int bottomMargin = 0)
        {
            InitializeComponent();
            Event = item;
            LeftMargin = leftMargin;
            TopMargin = topMargin;
            RightMargin = rightMargin;
            BottomMargin = bottomMargin;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == EventProperty.PropertyName)
            {
                text.Text = Event.Day.ToString();
                stack.BackgroundColor = Color.FromHex(Event.Heh);
                description.Text = Event.Description;
            }
            else if (propertyName == BorderBackgroundProperty.PropertyName)
                box.BackgroundColor = BorderBackground;
            else if (propertyName == LeftMarginProperty.PropertyName || propertyName == TopMarginProperty.PropertyName || propertyName == RightMarginProperty.PropertyName || propertyName == BottomMarginProperty.PropertyName)
            {
                Thickness margin = stack.Margin;
                margin.Left = LeftMargin;
                margin.Top = TopMargin;
                margin.Right = RightMargin;
                margin.Bottom = BottomMargin;
                stack.Margin = margin;
            }
        }
    }
}