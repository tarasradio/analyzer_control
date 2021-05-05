using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnalyzerControlGUI.Views.CustomViews
{
    /// <summary>
    /// Логика взаимодействия для CartridgeCassetteControl.xaml
    /// </summary>
    public partial class CartridgeCassetteControl : UserControl
    {
        readonly int _maxCount = 0;
        readonly double _maxHeight = 0;

        public static readonly DependencyProperty BarcodeProperty = DependencyProperty.Register(
            "Barcode", typeof(string), typeof(CartridgeCassetteControl), new PropertyMetadata("123", new PropertyChangedCallback(BarcodeChanged)));

        public static readonly DependencyProperty CountLeftProperty = DependencyProperty.Register(
            "CountLeft", typeof(float), typeof(CartridgeCassetteControl), new PropertyMetadata(5.0f, new PropertyChangedCallback(CountLeftChanged)));

        private static void BarcodeChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            CartridgeCassetteControl s = (CartridgeCassetteControl)depObj;
            s.LabelName.Content = args.NewValue.ToString();
        }

        private static void CountLeftChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            CartridgeCassetteControl s = (CartridgeCassetteControl)depObj;
            s.UpdateView();
        }

        public CartridgeCassetteControl()
        {
            InitializeComponent();
            _maxHeight = Status.Height;
            _maxCount = 10;
            UpdateView();
        }

        public CartridgeCassetteControl(int maxCount, int countLeft, string name)
        {
            InitializeComponent();

            _maxHeight = Status.Height;
            _maxCount = maxCount;

            CountLeft = countLeft;
            Barcode = name;
        }

        public string Barcode
        {
            get => (string)GetValue(BarcodeProperty);
            set
            {
                SetValue(BarcodeProperty, value);
            }
        }

        public float CountLeft
        {
            get => (float)GetValue(CountLeftProperty);
            set
            {
                if (value <= _maxCount) {
                    SetValue(CountLeftProperty, value);
                }
            }
        }

        void UpdateView()
        {
            Status.Height = CountLeft * _maxHeight / _maxCount;
            LabelCount.Content = CountLeft.ToString();

            if ((float)CountLeft / _maxCount <= 0.2) {
                Status.Fill = Brushes.LightPink;
            }
            else if ((float)CountLeft / _maxCount <= 0.6) {
                Status.Fill = Brushes.Khaki;
            }
            else if ((float)CountLeft / _maxCount <= 1) {
                Status.Fill = Brushes.LightGreen;
            }
        }
    }
}
