using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DateControl
{
    public partial class SvgPath
    {
        public static readonly BindableProperty DataProperty;
        public static readonly BindableProperty BorderColorProperty;

        public string Data
        {
            get => GetValue(DataProperty).ToString();
            set => SetValue(DataProperty, value);
        }
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        static SvgPath()
        {
            DataProperty = BindableProperty.Create(nameof(Data), typeof(string), typeof(SvgPath), "");
            BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SvgPath), Color.White);
        }

        public SvgPath(string data)
        {
            InitializeComponent();
            Data = data;
        }

        private void SKCanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);
            SKPath path = SKPath.ParseSvgPathData(Data);
            canvas.Translate(((float)view.Width - 30) / 2, ((float)view.Height - 30) / 2);
            canvas.DrawPath(path, new SKPaint() { Color = SKColors.Black });
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == BorderColorProperty.PropertyName)
                box.Color = BorderColor;
            else if (propertyName == DataProperty.PropertyName)
                view.InvalidateSurface();
        }
    }
}