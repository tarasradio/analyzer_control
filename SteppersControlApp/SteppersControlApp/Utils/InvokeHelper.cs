using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace SteppersControlApp.Utils
{
    public static class InvokeHelper
    {
        public static void InvokeThread(this Control control, Action action)
        {
            control.BeginInvoke(new MethodInvoker(action));
        }
    }
}
