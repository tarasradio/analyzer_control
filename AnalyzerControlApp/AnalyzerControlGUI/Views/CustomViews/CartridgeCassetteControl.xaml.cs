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
            "Barcode", typeof(string), typeof(CartridgeCassetteControl), new PropertyMetadata("Barcode", new PropertyChangedCallback(BarcodeChanged)));

        public static readonly DependencyProperty CountLeftProperty = DependencyProperty.Register(
            "CountLeft", typeof(int), typeof(CartridgeCassetteControl), new PropertyMetadata(5, new PropertyChangedCallback(CountLeftChanged)));

        private static void BarcodeChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            CartridgeCassetteControl s = (CartridgeCassetteControl)depObj;
            Label theLabel = s.LabelName;
            theLabel.Content = args.NewValue.ToString();
        }

        private static void CountLeftChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            CartridgeCassetteControl s = (CartridgeCassetteControl)depObj;
            s.CountLeft = (int)args.NewValue;
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
                LabelName.Content = value;
            }
        }

        public int CountLeft
        {
            get => (int)GetValue(CountLeftProperty);
            set
            {
                if (value <= _maxCount)
                {
                    SetValue(CountLeftProperty, value); ;
                    UpdateView();
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
