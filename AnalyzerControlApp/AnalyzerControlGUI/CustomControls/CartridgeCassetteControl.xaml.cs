using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnalyzerControlGUI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CartridgeCassetteControl.xaml
    /// </summary>
    public partial class CartridgeCassetteControl : UserControl
    {
        readonly int _maxCount = 0;
        readonly double _maxHeight = 0;
        int _countLeft = 0;
        string _name;

        public CartridgeCassetteControl(int maxCount, int countLeft, string name)
        {
            InitializeComponent();

            _maxHeight = Status.Height;
            _maxCount = maxCount;

            CountLeft = countLeft;
            LogoName = name;
        }

        public string LogoName
        {
            get => _name;
            set {
                _name = value;
                LabelName.Content = _name;
            }
        }

        public int CountLeft
        {
            get => _countLeft;
            set {
                if (value <= _maxCount) {
                    _countLeft = value;
                    UpdateView();
                }
            }
        }

        void UpdateView()
        {
            Status.Height = _countLeft * _maxHeight / _maxCount;
            LabelCount.Content = _countLeft.ToString();
            if ((float)_countLeft / _maxCount <= 0.2)
            {
                Status.Fill = Brushes.LightPink;
            }
            else if ((float)_countLeft / _maxCount <= 0.6)
            {
                Status.Fill = Brushes.Khaki;
            }
            else if ((float)_countLeft / _maxCount <= 1)
            {
                Status.Fill = Brushes.LightGreen;
            }
        }
    }
}
