using System;

using System.Windows.Forms;

namespace PresentationWinForms.Utils
{
    public static class InvokeHelper
    {
        public static void InvokeThread(this Control control, Action action)
        {
            control.BeginInvoke(new MethodInvoker(action));
        }
    }
}
