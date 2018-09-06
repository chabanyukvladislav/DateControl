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
	        get => (Color) GetValue(ColorProperty);
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
		    InitializeColors();
		}

        private void ExecuteSelect(object obj)
        {
            BoxView box = (BoxView) obj;
            Color = box.Color;
        }

        private void InitializeColors()
        {
            Color[] colors = {Color.Blue, Color.GreenYellow, Color.BlueViolet, Color.LightBlue, Color.Yellow, Color.Orange, Color.Pink, Color.White, Color.Red, Color.OrangeRed};
            int row = 1;
            int column = 0;
            for (int i = 0; i < 10; i++)
            {
                BoxView box = new BoxView()
                {
                    Color = colors[i]
                };
                box.GestureRecognizers.Add(new TapGestureRecognizer(){Command = Select, CommandParameter = box});
                This.Children.Add(box, column, row);
                column++;
                if (column == 5)
                {
                    row++;
                    column = 0;
                }
            }
        }

	    protected override void OnPropertyChanged(string propertyName = null)
	    {
	        base.OnPropertyChanged(propertyName);

	        if (propertyName == ColorProperty.PropertyName)
	        {
	            view.Color = Color;
	        }
        }
	}
}