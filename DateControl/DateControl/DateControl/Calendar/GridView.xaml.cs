using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace DateControl.Calendar
{
    public partial class GridView
    {
        public static readonly BindableProperty ItemsSourceProperty;
        public static readonly BindableProperty ColumnCountProperty;
        public static readonly BindableProperty RowCountProperty;

        public ICommand Select { get; set; }

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
            Select = new Command(ExecuteSelect);
            InitializeComponent();
            InitializeView();
        }

        private void ExecuteSelect(object obj)
        {
            foreach (GridViewCell child in gridView.Children)
            {
                child.BorderBackground = Color.Black;
                child.LeftMargin = 0;
                child.TopMargin = 1;
                child.RightMargin = 1;
                child.BottomMargin = 0;
            }
            GridViewCell cell = obj as GridViewCell;
            cell.BorderBackground = Color.Red;
            cell.LeftMargin = 2;
            cell.TopMargin = 2;
            cell.RightMargin = 2;
            cell.BottomMargin = 2;
            SelectedChanged?.Invoke(cell.Event);
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
            foreach (Event item in ItemsSource)
            {
                GridViewCell cell;
                if (column == ColumnCount - 1)
                    cell = new GridViewCell(item, 0, 1, 0, 0);
                else
                    cell = new GridViewCell(item);
                TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer
                {
                    Command = Select
                };
                cell.GestureRecognizers.Add(gestureRecognizer);
                gestureRecognizer.CommandParameter = cell;
                gridView.Children.Add(cell, column, row);
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

        public event Action<Event> SelectedChanged;
    }
}