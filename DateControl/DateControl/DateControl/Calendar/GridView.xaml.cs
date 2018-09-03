using System.Collections;
using Xamarin.Forms;

namespace DateControl.Calendar
{
    public partial class GridView
    {
        public static readonly BindableProperty ItemsSourceProperty;

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        static GridView()
        {
            ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(GridView));
        }

        public GridView()
        {
            InitializeComponent();
        }
    }
}