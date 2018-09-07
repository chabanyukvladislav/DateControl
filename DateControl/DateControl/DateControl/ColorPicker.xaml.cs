using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DateControl
{
    public partial class ColorPicker
    {
        public static readonly BindableProperty ColorProperty;

        public ICommand Select { get; set; }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        static ColorPicker()
        {
            ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(ColorPicker), Color.White, BindingMode.TwoWay);
        }

        public ColorPicker()
        {
            Select = new Command(ExecuteSelect);
            InitializeComponent();
            InitializeDefinitions(5, 2);
            InitializeColors();
        }

        private void ExecuteSelect(object obj)
        {
            if (obj == null)
                return;
            foreach (ColorCell child in This.Children)
            {
                child.BorderColor = Color.White;
            }
            ColorCell box = (ColorCell)obj;
            Color = box.Color;
            box.BorderColor = Color.Black;
        }

        private void InitializeColors()
        {
            Color[] colors = { Color.Blue, Color.GreenYellow, Color.BlueViolet, Color.LightBlue, Color.Yellow, Color.Orange, Color.Pink, Color.White, Color.Red, Color.OrangeRed };
            int row = 0;
            int column = 0;
            for (int i = 0; i < 10; i++)
            {
                ColorCell cell = new ColorCell(colors[i]);
                if(cell.Color == Color.White)
                    cell.BorderColor = Color.Black;
                cell.GestureRecognizers.Add(new TapGestureRecognizer() { Command = Select, CommandParameter = cell });
                This.Children.Add(cell, column, row);
                column++;
                if (column == 5)
                {
                    row++;
                    column = 0;
                }
            }
        }

        private void InitializeDefinitions(int columnCount, int rowCount)
        {
            InitializeColumnDefinitions(columnCount);
            InitializeRowDefinitions(rowCount);
        }

        private void InitializeColumnDefinitions(int columnCount)
        {
            ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
            ColumnDefinition columnDefinition = new ColumnDefinition
            {
                Width = new GridLength(50)
            };
            int count = 0;
            while (count != columnCount)
            {
                columnDefinitions.Add(columnDefinition);
                count++;
            }
            This.ColumnDefinitions = columnDefinitions;
        }

        private void InitializeRowDefinitions(int rowCount)
        {
            RowDefinitionCollection rowDefinitions = new RowDefinitionCollection();
            RowDefinition rowDefinition = new RowDefinition
            {
                Height = new GridLength(50)
            };
            int count = 0;
            while (count != rowCount)
            {
                rowDefinitions.Add(rowDefinition);
                count++;
            }
            This.RowDefinitions = rowDefinitions;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ColorProperty.PropertyName)
                ExecuteSelect(This.Children.FirstOrDefault(el => ((ColorCell)el).Color == Color));
        }
    }
}