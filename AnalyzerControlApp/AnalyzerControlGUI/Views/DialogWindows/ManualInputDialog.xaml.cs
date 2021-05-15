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
using System.Windows.Shapes;
using WPFTabTip;

namespace AnalyzerControlGUI.Views.DialogWindows
{
    /// <summary>
    /// Interaction logic for ManualInputDialog.xaml
    /// </summary>
    public partial class ManualInputDialog : Window
    {
        public ManualInputDialog()
        {
            InitializeComponent();
            TabTipAutomation.IgnoreHardwareKeyboard = HardwareKeyboardIgnoreOptions.IgnoreAll;
            TabTipAutomation.BindTo<TextBox>();
            TabTipAutomation.BindTo<RichTextBox>();
            TabTipAutomation.ExceptionCatched += TabTipAutomationOnTest;
        }

        private void TabTipAutomationOnTest(Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}
