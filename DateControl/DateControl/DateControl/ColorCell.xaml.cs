using Xamarin.Forms;

namespace DateControl
{
    public partial class ColorCell
    {
        public static readonly BindableProperty ColorProperty;
        public static readonly BindableProperty BorderColorProperty;

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        static ColorCell()
        {
            ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(ColorCell), Color.White);
            BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ColorCell), Color.White);
        }

        public ColorCell(Color color)
        {
            InitializeComponent();
            Color = color;
            box.Color = Color;
            back.Color = BorderColor;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ColorProperty.PropertyName)
                box.Color = Color;
            else if (propertyName == BorderColorProperty.PropertyName)
                back.Color = BorderColor;
        }
    }
}