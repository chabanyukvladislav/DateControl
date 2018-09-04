using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DateControl.Calendar
{
    public partial class GridView
    {
        public static readonly BindableProperty ItemsSourceProperty;
        public static readonly BindableProperty ColumnCountProperty;
        public static readonly BindableProperty RowCountProperty;

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public int ColumnCount
        {
            get => (int)GetValue(ColumnCountProperty);
            set => SetValue(ColumnCountProperty, value);
        }
        public int RowCount
        {
            get => (int)GetValue(RowCountProperty);
            set => SetValue(RowCountProperty, value);
        }

        static GridView()
        {
            ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(GridView), new List<object>());
            ColumnCountProperty = BindableProperty.Create(nameof(ColumnCount), typeof(int), typeof(GridView), 0);
            RowCountProperty = BindableProperty.Create(nameof(RowCount), typeof(int), typeof(GridView), 0);
        }

        public GridView()
        {
            InitializeComponent();
            InitializeView();
        }

        private void InitializeView()
        {
            InitializeDefinitions();
            InitializeItems();
        }

        private void InitializeItems()
        {
            int row = 0;
            int column = 0;
            foreach (object item in ItemsSource)
            {
                BoxView box = new BoxView
                {
                    BackgroundColor = Color.Black
                };
                StackLayout stack = new StackLayout()
                {
                    Margin = new Thickness(0, 1, 1, 0),
                    BackgroundColor = Color.White
                };
                Label view = new Label
                {
                    Text = item.ToString(),
                    Margin = new Thickness(8, 8, 0, 0),
                    BackgroundColor = Color.White
                };
                stack.Children.Add(view);
                gridView.Children.Add(box, column, row);
                gridView.Children.Add(stack, column, row);
                column++;
                if (column == ColumnCount)
                {
                    column = 0;
                    row++;
                }
            }
        }

        private void InitializeDefinitions()
        {
            InitializeColumnDefinitions();
            InitializeRowDefinitions();
        }

        private void InitializeColumnDefinitions()
        {
            ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
            ColumnDefinition columnDefinition = new ColumnDefinition
            {
                Width = GridLength.Star
            };
            int count = 0;
            while (count != ColumnCount)
            {
                columnDefinitions.Add(columnDefinition);
                count++;
            }
            gridView.ColumnDefinitions = columnDefinitions;
        }

        private void InitializeRowDefinitions()
        {
            RowDefinitionCollection rowDefinitions = new RowDefinitionCollection();
            RowDefinition rowDefinition = new RowDefinition
            {
                Height = GridLength.Star
            };
            int count = 0;
            while (count != RowCount)
            {
                rowDefinitions.Add(rowDefinition);
                count++;
            }
            gridView.RowDefinitions = rowDefinitions;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == RowCountProperty.PropertyName || propertyName == ColumnCountProperty.PropertyName || propertyName == ItemsSourceProperty.PropertyName)
                InitializeView();
        }
    }
}